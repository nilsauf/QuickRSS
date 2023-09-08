namespace QuickRSS.Api.Models
{
    using QuickRSS.Entities;
    using System.ComponentModel.DataAnnotations;

    public class FeedSettingsModel
    {
        public Guid? Id { get; set; }
        public string? Title { get; set; }

        [Required(ErrorMessage = "Url is required")]
        [Url]
        public required string Url { get; set; }

        [Required(ErrorMessage = "RefreshRate in ms is required")]
        public double RefreshRateMilliseconds { get; set; }

        public int MaxItemCount { get; set; } = 25;

        public Feed ToEntity()
        {
            var newFeed = new Feed
            {
                URL = this.Url,
                RefreshRate = TimeSpan.FromMilliseconds(this.RefreshRateMilliseconds),
                MaxItemCount = this.MaxItemCount,
            };

            if (this.Id.HasValue)
            {
                newFeed.Id = this.Id.Value;
            }

            return newFeed;
        }

        public static FeedSettingsModel FromEntity(Feed entity)
            => new FeedSettingsModel()
            {
                Id = entity.Id,
                Title = entity.Title,
                Url = entity.URL,
                RefreshRateMilliseconds = entity.RefreshRate.TotalMilliseconds,
                MaxItemCount = entity.MaxItemCount,
            };
    }
}
