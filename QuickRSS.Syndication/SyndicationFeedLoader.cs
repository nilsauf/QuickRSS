namespace QuickRSS.Syndication
{
    using Microsoft.Extensions.Logging;
    using QuickRSS.Entitites;
    using QuickRSS.Logic;
    using System;
    using System.Linq;
    using System.ServiceModel.Syndication;
    using System.Xml;

    public class SyndicationFeedLoader : IFeedLoader
    {
        private readonly ILogger<SyndicationFeedLoader> logger;

        public SyndicationFeedLoader(ILogger<SyndicationFeedLoader> logger)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Feed> LoadAsync(string url, CancellationToken cancellationToken)
        {
            using var feedLogger = this.logger.BeginScope("Load feed from '{url}'", url);
            this.logger.LogInformation("Starting");

            var internalFeed = await Task.Run(() =>
            {
                using var reader = XmlReader.Create(url);
                return SyndicationFeed.Load(reader);
            }, cancellationToken);

            this.logger.LogInformation("Finishing");

            return new Feed()
            {
                Title = internalFeed.Title.Text,
                URL = url,
                Items = internalFeed.Items.Select(item => new FeedItem()
                {
                    Title = item.Title.Text,
                    URL = item.Links?.FirstOrDefault()?.ToString() ?? "No Link!",
                    Content = item.Summary.ToString() ?? "No Content!",
                }).ToList(),
            };
        }
    }
}
