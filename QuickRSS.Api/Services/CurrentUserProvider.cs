namespace QuickRSS.Api.Services
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using QuickRSS.Database.EFCore;
    using QuickRSS.Database.User;
    using QuickRSS.Entities;
    using System.Threading.Tasks;

    public class CurrentUserProvider : ICurrentUserProvider
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly UserManager<ApplicationUser> userManager;

        public CurrentUserProvider(
            IHttpContextAccessor httpContextAccessor,
            UserManager<ApplicationUser> userManager)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.userManager = userManager;
        }

        public async Task<QuickRssUser?> GetAsync(
            bool includeFeedItems = false,
            CancellationToken cancellationToken = default)
        {
            var userClaim = this.httpContextAccessor.HttpContext?.User;
            if (userClaim is null)
                return null;

            var userId = this.userManager.GetUserId(userClaim);

            var users = this.userManager.Users;
            users = includeFeedItems ?
                users.Include(u => u.QuickRssUser.Feeds)
                    .ThenInclude(f => f.Items) :
                users;

            var appUser = await users.FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

            if (appUser is null)
                return null;

            return appUser.QuickRssUser;
        }
    }
}
