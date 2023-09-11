namespace QuickRSS.Api.Extensions
{
    using QuickRSS.Api.Models;
    using QuickRSS.Entities;

    public static class FeedExtensions
    {
        public static Feed ToEntity(this FeedSettingsModel settings)
        {
            var newFeed = new Feed
            {
                URL = settings.Url,
                RefreshRate = TimeSpan.FromMilliseconds(settings.RefreshRateMilliseconds),
                MaxItemCount = settings.MaxItemCount,
            };

            if (settings.Id.HasValue)
            {
                newFeed.Id = settings.Id.Value;
            }

            return newFeed;
        }

        public static FeedSettingsModel ToSettingsModel(this Feed entity)
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
