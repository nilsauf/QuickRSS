namespace QuickRSS.Api.Extensions
{
	using Microsoft.AspNetCore.Authentication.JwtBearer;
	using Microsoft.AspNetCore.Identity;
	using Microsoft.EntityFrameworkCore;
	using Microsoft.Extensions.Configuration;
	using Microsoft.Extensions.DependencyInjection;
	using Microsoft.IdentityModel.Tokens;
	using QuickRSS.Api.Services;
	using QuickRSS.CodeHollowFeedReader;
	using QuickRSS.Database;
	using QuickRSS.Database.EFCore;
	using QuickRSS.Database.Feed;
	using QuickRSS.Database.FeedItem;
	using QuickRSS.Database.User;
	using QuickRSS.Logic;
	using System.Text;

	public static class IServiceCollectionExtensions
	{
		public static IServiceCollection AddQuickRss(this IServiceCollection services, IConfiguration configuration)
		{
			string connectionString = configuration.GetConnectionString("DefaultConnection")
				?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

			return services.AddTransient<FeedAggregator>()
				.AddTransient<IFeedLoader, CodeHollowFeedLoader>()
				.AddTransient<IFeedProvider, FeedProvider>()
				.AddTransient<IFeedUpdater, FeedUpdater>()
				.AddTransient<IFeedStore, FeedStore>()
				.AddTransient<IFeedItemStore, FeedItemStore>()
				.AddHostedService<FeedAggregatorService>()
				.AddSingleton<IFeedChangeNotifier, FeedChangeNotifier>()
				.AddDbContext<IQuickRssDataAccess, QuickRssDbContext>(
					options => options.ConfigureForEFCore(connectionString))
				.AddTransient<IQuickRssUserStore, QuickRssUserStore>()
				.AddTransient<ICurrentUserProvider, CurrentUserProvider>()
				.AddSingleton<IEqualityComparer<Guid>>(EqualityComparer<Guid>.Default)
				.AddAuthentication(configuration)
				.AddQuickRssIdentity();
		}

		public static IServiceCollection AddQuickRssIdentity(this IServiceCollection services)
		{
			services.AddIdentity<ApplicationUser, IdentityRole>()
				.AddEntityFrameworkStores<QuickRssDbContext>()
				.AddDefaultTokenProviders();

			return services;
		}

		public static IServiceCollection AddAuthentication(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
			})
			.AddJwtBearer(options =>
			{
				options.SaveToken = true;
				options.RequireHttpsMetadata = false;
				options.TokenValidationParameters = new TokenValidationParameters()
				{
					ValidateIssuer = true,
					ValidateAudience = true,
					ValidAudience = configuration["JWT:ValidAudience"],
					ValidIssuer = configuration["JWT:ValidIssuer"],
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]!))
				};
			});

			return services;
		}
	}
}
