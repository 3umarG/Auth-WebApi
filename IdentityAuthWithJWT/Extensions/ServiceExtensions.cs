using IdentityAuthWithJWT.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityAuthWithJWT.Extensions
{
	public static class ServiceExtensions
	{
		public static void ConfigureIdentity(this IServiceCollection services)
		{
			/*
			 * 
			   services.AddIdentity<IdentityUser, IdentityRole>(options =>
				{
					options.User.RequireUniqueEmail = false;
				})
				.AddEntityFrameworkStores<Providers.Database.EFProvider.DataContext>()
				.AddDefaultTokenProviders();
			 */
			var builder = services.AddIdentityCore<ApiUser>();

			builder = new IdentityBuilder(builder.UserType, typeof(IdentityRole) , services);

			builder.AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();
		} 
	}
}
