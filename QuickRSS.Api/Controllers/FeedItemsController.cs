namespace QuickRSS.Api.Controllers
{
	using Microsoft.AspNetCore.Authorization;
	using Microsoft.AspNetCore.Mvc;
	using QuickRSS.Database.Feed;
	using QuickRSS.Entities;

	[Route("api/feed/items")]
	[ApiController]
	[Authorize]
	public class FeedItemsController : ControllerBase
	{
		private readonly IFeedStore feedStore;

		public FeedItemsController(IFeedStore feedStore)
		{
			this.feedStore = feedStore;
		}

		[HttpGet]
		public async Task<IActionResult> Get(Guid feed, string? item)
		{
			var feedEntity = await feedStore.GetAsync(feed, true);
			if (feedEntity is null)
			{
				return NotFound();
			}

			return string.IsNullOrWhiteSpace(item) ?
				GetAllItems(feedEntity) :
				GetItem(feedEntity, item);

			IActionResult GetAllItems(Feed feed) => Ok(feed.Items);

			IActionResult GetItem(Feed feed, string itemId)
			{
				var feedItem = feed.Items.FirstOrDefault(item => item.Id == itemId);
				return feedItem is null ? NotFound() : Ok(feedItem);
			}
		}
	}
}
