namespace QuickRSS.CodeHollowFeedReader
{
    using CodeHollow.FeedReader;
    using Microsoft.Extensions.Logging;
    using QuickRSS.Logic;
    using System;
    using System.Threading.Tasks;

    public class CodeHollowFeedLoader : IFeedLoader
    {
        private readonly ILogger<CodeHollowFeedLoader> logger;

        public CodeHollowFeedLoader(ILogger<CodeHollowFeedLoader> logger)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Entities.Feed> LoadAsync(string url, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                throw new ArgumentException($"'{nameof(url)}' cannot be null or whitespace.", nameof(url));
            }

            cancellationToken.ThrowIfCancellationRequested();

            using var feedLogger = this.logger.BeginScope("Load feed from '{url}'", url);
            this.logger.LogInformation("Starting");

            var internalFeed = await FeedReader.ReadAsync(url, cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();

            if (internalFeed is null)
                throw new Exception("Feed could not be loaded!");

            var feed = internalFeed.ToEntity();

            this.logger.LogInformation("Finishing");

            return feed;
        }
    }
}
