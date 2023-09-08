namespace QuickRSS.Database.EFCore
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Design;

    public class QuickRssDbContextFactory : IDesignTimeDbContextFactory<QuickRssDbContext>
    {
        public QuickRssDbContext CreateDbContext(string[] args)
        {
            var optionsBuiler = new DbContextOptionsBuilder<QuickRssDbContext>()
                .UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=quickrss-dev;Trusted_Connection=True;MultipleActiveResultSets=true")
                .EnableSensitiveDataLogging();
            return new QuickRssDbContext(optionsBuiler.Options);

        }
    }
}
