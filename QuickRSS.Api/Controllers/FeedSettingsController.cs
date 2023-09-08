namespace QuickRSS.Api.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using QuickRSS.Api.Models;
    using QuickRSS.Database.Feed;

    [Route("api/feed/settings")]
    [ApiController]
    [Authorize]
    public class FeedSettingsController : ControllerBase
    {
        private readonly IFeedStore feedStore;

        public FeedSettingsController(IFeedStore feedStore)
        {
            this.feedStore = feedStore;
        }

        [HttpGet()]
        public Task<IActionResult> Get(Guid? id)
        {
            return id is Guid notNullId ?
                GetById(notNullId) :
                GetAll();

            async Task<IActionResult> GetById(Guid id)
            {
                var feed = await this.feedStore.GetAsync(notNullId);
                return feed is null ?
                    StatusCode(StatusCodes.Status500InternalServerError) :
                    Ok(FeedSettingsModel.FromEntity(feed));
            }

            async Task<IActionResult> GetAll()
            {
                var feeds = await this.feedStore.GetAllAsync();
                return feeds is null ?
                    StatusCode(StatusCodes.Status500InternalServerError) :
                    Ok(feeds.Select(FeedSettingsModel.FromEntity));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] FeedSettingsModel value)
        {
            var result = await this.feedStore.AddAsync(value.ToEntity());

            return result ? Ok() : StatusCode(StatusCodes.Status500InternalServerError);
        }

        [HttpPut()]
        public async Task<IActionResult> Put([FromBody] FeedSettingsModel value)
        {
            if (value.Id is null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { error = "No id given!" });
            }

            var newValues = value.ToEntity();
            var result = await this.feedStore.UpdateAsync(newValues.Id, feed =>
            {
                feed.Title = newValues.Title;
                feed.URL = newValues.URL;
                feed.RefreshRate = newValues.RefreshRate;
                feed.MaxItemCount = newValues.MaxItemCount;
            });

            return result ? Ok() : StatusCode(StatusCodes.Status500InternalServerError);
        }

        [HttpDelete()]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await this.feedStore.RemoveAsync(id);
            return result ? Ok() : StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}
