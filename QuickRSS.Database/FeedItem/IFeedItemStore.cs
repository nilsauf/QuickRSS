namespace QuickRSS.Database.FeedItem
{
	using QuickRSS.Entities;

	public interface IFeedItemStore
	{
		Task<FeedItem?> GetAsync(string id, CancellationToken cancellationToken = default);
		Task<bool> UpdateAsync(Action<FeedItem> update, params string[] ids);
	}
}
