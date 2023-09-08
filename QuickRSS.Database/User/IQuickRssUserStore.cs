namespace QuickRSS.Database.User
{
    using QuickRSS.Entities;

    public interface IQuickRssUserStore
    {
        Task<QuickRssUser?> GetAsync(Guid id);
        Task<List<QuickRssUser>> GetAllAsync();
    }
}
