namespace QuickRSS.Api.Services
{
    using Microsoft.AspNetCore.Identity;
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

        public async Task<QuickRssUser?> GetAsync()
        {
            var userClaim = this.httpContextAccessor.HttpContext?.User;
            if (userClaim is null)
                return null;

            var appUser = await this.userManager.GetUserAsync(userClaim);
            if (appUser is null)
                return null;

            return appUser.QuickRssUser;
        }
    }
}
