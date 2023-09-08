namespace QuickRSS.Api.Services
{
    using QuickRSS.Logic;
    using System.Threading;
    using System.Threading.Tasks;

    public class FeedAggregatorService : BackgroundService
    {
        private readonly FeedAggregator feedAggregator;

        public FeedAggregatorService(FeedAggregator feedAggregator)
        {
            this.feedAggregator = feedAggregator;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return feedAggregator.RunAsync(stoppingToken);
        }
    }
}
