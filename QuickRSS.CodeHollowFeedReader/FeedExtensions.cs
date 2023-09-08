namespace QuickRSS.CodeHollowFeedReader
{
    using QuickRSS.Entities;

    internal static class FeedExtensions
    {
        public static Feed ToEntity(this CodeHollow.FeedReader.Feed feed)
        {
            return new Feed()
            {
                Title = feed.Title,
                URL = feed.Link,
                Description = feed.Description,
                Language = feed.Language,
                Copyright = feed.Copyright,
                ImageUrl = feed.ImageUrl,
                Items = feed.Items.Select(ToEntity).ToList(),
            };
        }

        public static FeedItem ToEntity(this CodeHollow.FeedReader.FeedItem item)
        {
            return new FeedItem()
            {
                Id = item.Id,
                Title = item.Title,
                URL = item.Link,
                Content = item.Content,
                PublishingDate = item.PublishingDateString
            };
        }
    }
}
