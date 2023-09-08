namespace QuickRSS.Database.Feed
{
    using QuickRSS.Entities;
    using System;
    using System.Collections.Generic;

    public interface IFeedStore
    {
        Task<bool> AddAsync(Feed feed);
        Task<bool> RemoveAsync(Guid id);
        Task<Feed?> GetAsync(Guid id);
        Task<IEnumerable<Feed>?> GetAllAsync();
        Task<bool> UpdateAsync(Guid id, Action<Feed> update, CancellationToken token = default);
    }
}
