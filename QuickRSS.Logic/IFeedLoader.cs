namespace QuickRSS.Logic
{
    using QuickRSS.Entities;

    public interface IFeedLoader
    {
        public Task<Feed> LoadAsync(string url, CancellationToken cancellationToken);
    }
}
