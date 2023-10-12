namespace QuickRSS.Api.Controllers
{
	using Microsoft.AspNetCore.Authorization;
	using Microsoft.AspNetCore.Mvc;
	using QuickRSS.Database.Feed;
	using QuickRSS.Database.FeedItem;

	[Route("api/feed/items")]
	[ApiController]
	[Authorize]
	public class FeedItemsController : ControllerBase
	{
		private readonly IFeedStore feedStore;
		private readonly IFeedItemStore feedItemStore;

		public FeedItemsController(IFeedStore feedStore, IFeedItemStore feedItemStore)
		{
			this.feedStore = feedStore;
			this.feedItemStore = feedItemStore;
		}

		[HttpGet]
		public async Task<IActionResult> Get(Guid? feed, string? item)
		{
			return string.IsNullOrWhiteSpace(item) ?
				(feed.HasValue ?
					await GetAllItemsAsync(feed!.Value) :
					BadRequest()) :
				await GetItemAsync(item);

			async Task<IActionResult> GetAllItemsAsync(Guid feedId)
			{
				var feedEntity = await feedStore.GetAsync(feedId, true);
				return feedEntity is null ? NotFound() : Ok(feedEntity.Items);
			}

			async Task<IActionResult> GetItemAsync(string itemId)
			{
				var feedItem = await this.feedItemStore.GetAsync(itemId);
				return feedItem is null ? NotFound() : Ok(feedItem);
			}
		}
	}
}
