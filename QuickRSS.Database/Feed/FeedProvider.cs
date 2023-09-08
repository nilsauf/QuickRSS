namespace QuickRSS.Database.Feed
{
    using DynamicData;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using QuickRSS.Entities;
    using QuickRSS.Logic;
    using System;
    using System.Reactive.Concurrency;
    using System.Reactive.Disposables;
    using System.Reactive.Linq;

    public class FeedProvider : IFeedProvider
    {
        private readonly ILogger<FeedProvider> logger;
        private readonly IFeedChangeNotifier changeNotifier;
        private readonly IServiceScopeFactory scopeFactory;
        private readonly IEqualityComparer<Guid> guidComparer;

        public FeedProvider(
            ILogger<FeedProvider> logger,
            IFeedChangeNotifier changeNotifier,
            IServiceScopeFactory scopeFactory,
            IEqualityComparer<Guid> guidComparer)
        {
            this.logger = logger;
            this.changeNotifier = changeNotifier;
            this.scopeFactory = scopeFactory;
            this.guidComparer = guidComparer;
        }

        public IObservable<IChangeSet<Feed, Guid>> Connect()
        {
            return Observable.Create<IChangeSet<Feed, Guid>>(obs =>
            {
                var cache = new SourceCache<Feed, Guid>(f => f.Id);

                using var scope = this.scopeFactory.CreateScope();
                var dataAccess = scope.ServiceProvider.GetRequiredService<IQuickRssDataAccess>();

                var allFeeds = dataAccess.GetFeeds(true).ToList();
                cache.AddOrUpdate(allFeeds);

                var changeSub = this.changeNotifier.Changed
                    .Select(guid =>
                    {
                        using var scope = this.scopeFactory.CreateScope();
                        return (guid, feed: scope.ServiceProvider.GetRequiredService<IQuickRssDataAccess>()
                            .GetFeeds(true)
                            .FirstOrDefault(f => this.guidComparer.Equals(f.Id, guid)));
                    })
                    .Subscribe(data =>
                    {
                        if (data.feed is null)
                        {
                            cache.RemoveKey(data.guid);
                        }
                        else
                        {
                            cache.AddOrUpdate(data.feed);
                        }
                    }, obs.OnError);

                var cacheSub = cache.Connect()
                    .ObserveOn(TaskPoolScheduler.Default)
                    .Subscribe(obs);

                return new CompositeDisposable(
                    changeSub,
                    cacheSub,
                    cache);
            });
        }
    }
}
