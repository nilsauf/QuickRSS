namespace QuickRSS.Database.EFCore
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using QuickRSS.Database.User;
    using QuickRSS.Entities;
    using System;
    using System.Collections.Generic;

    public class QuickRssUserStore : IQuickRssUserStore
    {
        private readonly UserManager<ApplicationUser> userManager;

        public QuickRssUserStore(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }

        public async Task<QuickRssUser?> GetAsync(Guid id)
        {
            var user = await this.userManager.FindByIdAsync(id.ToString());
            return user?.QuickRssUser;
        }

        public Task<List<QuickRssUser>> GetAllAsync()
        {
            return this.userManager.Users
                .Include(u => u.QuickRssUser)
                .ThenInclude(rssUser => rssUser.Feeds)
                .Select(u => u.QuickRssUser)
                .ToListAsync();
        }
    }
}
