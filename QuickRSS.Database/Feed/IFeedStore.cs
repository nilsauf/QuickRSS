namespace QuickRSS.Database.Feed
{
    using QuickRSS.Entities;
    using System;
    using System.Collections.Generic;

    public interface IFeedStore
    {
        Task<bool> AddAsync(Feed feed);
        Task<bool> RemoveAsync(Guid id);
        Task<Feed?> GetAsync(Guid id,
            bool includeFeedItems = false,
            CancellationToken cancellationToken = default);
        Task<IEnumerable<Feed>?> GetAllAsync(
            bool includeFeedItems = false,
            CancellationToken cancellationToken = default);
        Task<bool> UpdateAsync(
            Guid id,
            Action<Feed> update,
            bool includeFeedItems = false);
    }
}
