namespace QuickRSS.Database.Feed
{
    using Microsoft.Extensions.DependencyInjection;
    using QuickRSS.Entities;
    using QuickRSS.Logic;
    using System.Reactive;

    public class FeedUpdater : IFeedUpdater
    {
        private readonly IServiceScopeFactory scopeFactory;

        public FeedUpdater(
            IServiceScopeFactory scopeFactory)
        {
            this.scopeFactory = scopeFactory;
        }

        public async ValueTask<Unit> UpdateFeedAsync(Feed feed)
        {
            using var scope = this.scopeFactory.CreateScope();
            var dataAccess = scope.ServiceProvider.GetRequiredService<IQuickRssDataAccess>();

            var updateFeed = dataAccess.GetFeeds()
                .FirstOrDefault(updateFeed => updateFeed.Id == feed.Id);

            if (updateFeed is not null)
            {
                UpdateFeed(updateFeed);
                await dataAccess.SaveChangesAsync();
            }

            return Unit.Default;

            void UpdateFeed(Feed updateFeed)
            {
                updateFeed.Title = feed.Title;
                updateFeed.Description = feed.Description;
                updateFeed.Copyright = feed.Copyright;
                updateFeed.Language = feed.Language;
                updateFeed.ImageUrl = feed.ImageUrl;

                var itemsBefore = updateFeed.Items;
                updateFeed.Items = updateFeed.Items
                    .Concat(feed.Items)
                    .DistinctBy(item => item.Id)
                    .OrderBy(item => DateTime.Parse(item.PublishingDate))
                    .Take(updateFeed.MaxItemCount)
                    .ToList();

                var feedsToAdd = updateFeed.Items
                    .Except(itemsBefore)
                    .ToArray();
                dataAccess.AddAsync(feedsToAdd);

                var feedsToRemove = itemsBefore.Except(updateFeed.Items)
                    .ToArray();
                dataAccess.Remove(feedsToRemove);
            }
        }
    }
}
