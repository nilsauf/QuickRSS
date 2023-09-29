namespace QuickRSS.Logic
{
    using DynamicData;
    using QuickRSS.Entities;
    using System;
    using System.Reactive.Concurrency;
    using System.Reactive.Linq;
    using System.Reactive.Threading.Tasks;

    public class FeedAggregator
    {
        private readonly IFeedLoader feedLoader;
        private readonly IFeedProvider feedProvider;
        private readonly IFeedUpdater feedUpdater;
        private readonly IScheduler scheduler;

        public FeedAggregator(
            IFeedLoader feedLoader,
            IFeedProvider feedProvider,
            IFeedUpdater feedUpdater,
            IScheduler? scheduler = null)
        {
            this.feedLoader = feedLoader ?? throw new ArgumentNullException(nameof(feedLoader));
            this.feedProvider = feedProvider ?? throw new ArgumentNullException(nameof(feedProvider));
            this.feedUpdater = feedUpdater ?? throw new ArgumentNullException(nameof(feedUpdater));
            this.scheduler = scheduler ?? Scheduler.Default;
        }

        public Task RunAsync(CancellationToken cancellationToken)
        {
            return this.feedProvider.Connect()
                .Transform(StartSingleFeedAggregation)
                .DisposeMany()
                .ToTask(cancellationToken);

            IDisposable StartSingleFeedAggregation(Feed feed)
            {
                return Observable.FromAsync(cancellationToken =>
                        this.feedLoader.LoadAsync(feed.URL, cancellationToken),
                        this.scheduler)
                    .DelaySubscription(feed.RefreshRate, this.scheduler)
                    .Do(fetchedFeed => fetchedFeed.Id = feed.Id)
                    .SelectMany(async fetchedFeed => await this.feedUpdater.UpdateFeedAsync(fetchedFeed))
                    .Repeat()
                    .Subscribe();
            }
        }
    }
}
