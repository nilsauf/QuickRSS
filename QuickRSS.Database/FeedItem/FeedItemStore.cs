namespace QuickRSS.Database.FeedItem
{
	using Microsoft.Extensions.Logging;
	using QuickRSS.Database.Feed;
	using QuickRSS.Entities;
	using System;
	using System.Linq;
	using System.Threading;
	using System.Threading.Tasks;

	public class FeedItemStore : IFeedItemStore
	{
		private readonly ILogger<FeedItemStore> logger;
		private readonly IFeedStore feedStore;
		private readonly IQuickRssDataAccess dataAccess;

		public FeedItemStore(
			ILogger<FeedItemStore> logger,
			IFeedStore feedStore,
			IQuickRssDataAccess dataAccess)
		{
			this.logger = logger;
			this.feedStore = feedStore;
			this.dataAccess = dataAccess;
		}

		public async Task<FeedItem?> GetAsync(string id, CancellationToken cancellationToken = default)
		{
			if (string.IsNullOrWhiteSpace(id))
				throw new ArgumentException($"'{nameof(id)}' cannot be null or whitespace.", nameof(id));

			var feeds = await this.feedStore.GetAllAsync();
			return feeds?.SelectMany(f => f.Items)
				.FirstOrDefault(fi => fi.Id.Equals(id));
		}

		public async Task<bool> UpdateAsync(Action<FeedItem> update, params string[] ids)
		{
			if (ids.Length == 0) throw new InvalidOperationException("No id's to update specified!");
			if (update is null) throw new ArgumentNullException(nameof(update));

			var feeds = await this.feedStore.GetAllAsync();
			if (feeds is null)
			{
				return false;
			}

			var feedItemsToUpdate = feeds.SelectMany(f => f.Items)
				.Where(fi => ids.Contains(fi.Id))
				.ToList();
			if (feedItemsToUpdate.Count == 0)
			{
				return false;
			}

			foreach (var item in feedItemsToUpdate)
			{
				update(item);
			}

			await this.dataAccess.SaveChangesAsync();
			return true;
		}
	}
}
