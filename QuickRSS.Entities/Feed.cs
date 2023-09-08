namespace QuickRSS.Entities
{
    using System.Collections.Generic;

    public class Feed
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string? Title { get; set; }
        public required string URL { get; set; }
        public string? Description { get; set; }
        public string? Language { get; set; }
        public string? Copyright { get; set; }
        public string? ImageUrl { get; set; }
        public TimeSpan RefreshRate { get; set; }
        public int MaxItemCount { get; set; }
        public IList<FeedItem> Items { get; set; } = new List<FeedItem>();
    }
}
