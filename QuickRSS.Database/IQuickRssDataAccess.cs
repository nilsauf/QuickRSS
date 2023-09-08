namespace QuickRSS.Database
{
    using System.Linq;

    public interface IQuickRssDataAccess
    {
        public IQueryable<Entities.Feed> GetFeeds(bool notTracking = false);
        public Task<bool> AddAsync(params object[] entities);
        public bool Remove(params object[] entities);
        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
