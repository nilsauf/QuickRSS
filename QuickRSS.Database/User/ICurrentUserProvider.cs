namespace QuickRSS.Database.User
{
    using QuickRSS.Entities;

    public interface ICurrentUserProvider
    {
        Task<QuickRssUser?> GetAsync();
    }
}
