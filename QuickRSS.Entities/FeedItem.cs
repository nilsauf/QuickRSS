namespace QuickRSS.Entities
{
    public record FeedItem
    {
        public string Id { get; set; }
        public string Title { get; init; }
        public string Content { get; init; }
        public string URL { get; init; }
        public string PublishingDate { get; init; }
    }
}
