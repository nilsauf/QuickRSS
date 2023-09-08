namespace QuickRSS.Database.EFCore
{
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using QuickRSS.Entities;
    using System.Linq;

    public class QuickRssDbContext : IdentityDbContext<ApplicationUser>, IQuickRssDataAccess
    {
        public QuickRssDbContext(DbContextOptions<QuickRssDbContext> options) : base(options)
        {

        }

        public DbSet<Feed> Feeds { get; set; }

        async Task<bool> IQuickRssDataAccess.AddAsync(params object[] entities)
        {
            try
            {
                await this.AddRangeAsync(entities);
                return true;
            }
            catch
            {
                return false;
            }
        }

        bool IQuickRssDataAccess.Remove(params object[] entities)
        {
            try
            {
                this.RemoveRange(entities);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<ApplicationUser>().Navigation(u => u.QuickRssUser).AutoInclude();
            builder.Entity<QuickRssUser>().Navigation(u => u.Feeds).AutoInclude();
            base.OnModelCreating(builder);
        }

        public IQueryable<Feed> GetFeeds(bool notTracking = false)
            => (notTracking ?
                    this.Feeds.AsNoTracking() :
                    this.Feeds)
                .Include(feed => feed.Items);
    }
}
