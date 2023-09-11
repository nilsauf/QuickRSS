namespace QuickRSS.Database.Feed
{
    using Microsoft.Extensions.Logging;
    using QuickRSS.Database;
    using QuickRSS.Database.Extensions;
    using QuickRSS.Database.User;
    using QuickRSS.Entities;
    using System;
    using System.Collections.Generic;

    public class FeedStore : IFeedStore
    {
        private readonly ILogger<FeedStore> logger;
        private readonly IQuickRssDataAccess dataAccess;
        private readonly IFeedChangeNotifier changeNotifier;
        private readonly ICurrentUserProvider userProvider;
        private readonly IEqualityComparer<Guid> guidComparer;

        public FeedStore(
            ILogger<FeedStore> logger,
            IQuickRssDataAccess dataAccess,
            IFeedChangeNotifier changeNotifier,
            ICurrentUserProvider userProvider,
            IEqualityComparer<Guid> guidComparer)
        {
            this.logger = logger;
            this.dataAccess = dataAccess;
            this.changeNotifier = changeNotifier;
            this.userProvider = userProvider;
            this.guidComparer = guidComparer;
        }

        public async Task<bool> AddAsync(Feed feed)
        {
            var user = await this.userProvider.GetAsync();
            if (user is null)
                return false;

            user.Feeds.Add(feed);
            await this.dataAccess.AddAsync(feed);
            await this.SaveAndNotifyChangesAsync(feed.Id);
            return true;
        }

        public async Task<bool> RemoveAsync(Guid id)
        {
            var user = await this.userProvider.GetAsync();
            if (user is null)
                return false;

            var feed = user.Feeds.FirstOrDefault(feed => guidComparer.Equals(feed.Id, id));
            if (feed is null)
                return false;

            user.Feeds.Remove(feed);
            this.dataAccess.Remove(feed);
            await this.SaveAndNotifyChangesAsync(id);
            return true;
        }

        public async Task<Feed?> GetAsync(
            Guid id,
            bool includeFeedItems = false,
            CancellationToken cancellationToken = default)
        {
            var user = await this.userProvider.GetAsync(includeFeedItems, cancellationToken);
            if (user is null)
                return null;

            return user.Feeds.FirstOrDefault(feed => guidComparer.Equals(feed.Id, id));
        }

        public async Task<IEnumerable<Feed>?> GetAllAsync(
            bool includeFeedItems = false,
            CancellationToken cancellationToken = default)
        {
            var user = await this.userProvider.GetAsync(includeFeedItems, cancellationToken);
            return user is null ?
                null :
                user.Feeds.ToList();
        }

        public async Task<bool> UpdateAsync(
            Guid id,
            Action<Feed> update,
            bool includeFeedItems = false)
        {
            if (update is null)
                throw new ArgumentNullException(nameof(update));

            var feed = await this.GetAsync(id, includeFeedItems);
            if (feed is null)
                return false;

            update(feed);

            await this.SaveAndNotifyChangesAsync(id);
            return true;
        }

        private async Task SaveAndNotifyChangesAsync(Guid id, CancellationToken token = default)
        {
            await this.dataAccess.SaveChangesAsync(token);
            this.changeNotifier.Notify(id);
        }
    }
}
