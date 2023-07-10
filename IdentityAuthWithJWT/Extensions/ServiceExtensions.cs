using IdentityAuthWithJWT.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityAuthWithJWT.Extensions
{
	public static class ServiceExtensions
	{
		public static void ConfigureIdentity(this IServiceCollection services)
		{
			var builder = services.AddIdentityCore<ApiUser>();

			builder = new IdentityBuilder(builder.UserType, typeof(IdentityRole) , services);

			builder.AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();
		} 
	}
}
