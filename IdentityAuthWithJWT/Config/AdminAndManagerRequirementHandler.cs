using Microsoft.AspNetCore.Authorization;

namespace IdentityAuthWithJWT.Config
{
	public class AdminAndManagerRequirementHandler : AuthorizationHandler<AdminAndManagerRequirement>
	{
		protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AdminAndManagerRequirement requirement)
		{
			var user = context.User;
			if (user.IsInRole("Admin") && user.IsInRole("Manager"))
			{
				context.Succeed(requirement);
			}

			return Task.CompletedTask;
		}
	}

	// Register the custom requirement handler to configure it on the Program.cs 
	public static class AuthorizationServiceExtensions
	{
		public static void AddAdminAndManagerRequirement(this IServiceCollection services)
		{
			services.AddSingleton<IAuthorizationHandler, AdminAndManagerRequirementHandler>();
		}
	}

}
