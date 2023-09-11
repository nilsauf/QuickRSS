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
        public async Task<IActionResult> Get(Guid feed, Guid? item)
        {
            var feedEntity = await feedStore.GetAsync(feed);
            if (feedEntity is null)
            {
                return NotFound();
            }

            return item.HasValue ?
                GetItem(feedEntity, item.Value.ToString()) :
                GetAllItems(feedEntity);

            IActionResult GetAllItems(Feed feed) => Ok(feed.Items);

            IActionResult GetItem(Feed feed, string itemId)
            {
                var feedItem = feed.Items.FirstOrDefault(item => item.Id == itemId);
                if (feedItem is null)
                    return NotFound();

                return Ok(feedItem);
            }
        }
    }
}
