namespace QuickRSS.Api.Test.Logic
{
    using DynamicData;
    using Microsoft.Reactive.Testing;
    using Moq;
    using QuickRSS.Entities;
    using QuickRSS.Logic;
    using System.Reactive;
    using System.Reactive.Concurrency;
    using Xunit;

    public class FeedAggregatorTests
    {
        private const string testingURL = nameof(testingURL);
        private readonly Feed testingFeed = new()
        {
            Id = Guid.NewGuid(),
            URL = testingURL,
            Title = nameof(testingFeed),
            RefreshRate = TimeSpan.FromTicks(100),
            Items = Enumerable.Range(0, 5)
                .Select(i => new FeedItem()
                {
                    Id = i.ToString(),
                    Title = $"Title_{i}",
                    Content = $"Content_{i}"
                })
                .ToList()
        };

        [Fact]
        public void Run()
        {
            var scheduler = new TestScheduler();
            var tokenSource = new CancellationTokenSource();
            using var testCache = new SourceCache<Feed, Guid>(f => f.Id);

            var feedLoaderMock = new Mock<IFeedLoader>();
            feedLoaderMock.Setup(fl => fl.LoadAsync(
                    It.IsNotNull<string>(),
                    It.IsNotNull<CancellationToken>()))
                .ReturnsAsync(testingFeed)
                .Verifiable(Times.Exactly(5));

            var feedProviderMock = new Mock<IFeedProvider>();
            feedProviderMock.Setup(fp => fp.Connect())
                .Returns(testCache.Connect())
                .Verifiable(Times.Once());

            var feedUpdaterMock = new Mock<IFeedUpdater>();
            feedUpdaterMock.Setup(fu => fu.UpdateFeedAsync(It.IsNotNull<Feed>()))
                .ReturnsAsync(Unit.Default)
                .Callback<Feed>(feed => Assert.Equal(feed.Id, testingFeed.Id))
                .Verifiable(Times.Exactly(5));

            var aggregator = new FeedAggregator(
                feedLoaderMock.Object,
                feedProviderMock.Object,
                feedUpdaterMock.Object,
                scheduler);

            var runTask = aggregator.RunAsync(tokenSource.Token);

            scheduler.ScheduleAbsolute(testingFeed, 500, (scheduler, feed) =>
            {
                testCache.AddOrUpdate(feed);
                return scheduler.Schedule(TimeSpan.FromTicks(550), () => testCache.Remove(feed));
            });

            scheduler.Start();
            scheduler.AdvanceBy(1500);

            feedLoaderMock.Verify();
            feedProviderMock.Verify();
            feedUpdaterMock.Verify();

            tokenSource.Cancel();
            Assert.True(runTask.IsCanceled);
            Assert.True(runTask.IsCompleted);
        }
    }
}
