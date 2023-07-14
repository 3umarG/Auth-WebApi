using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace IdentityAuthWithJWT.Data
{
	public class ApplicationDbContext :IdentityDbContext<ApiUser>
	{
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);

			builder.Entity<ApiUser>().OwnsMany(
				p => p.RefreshTokens, a =>
				{
					a.WithOwner().HasForeignKey("UserId");
					a.Property<int>("Id");
					a.HasKey("Id");
				});
			builder.ApplyConfiguration(new RoleConfig());

			builder.Entity<ApiUser>().ToTable("Users");
			builder.Entity<IdentityRole>().ToTable("Roles");
			builder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");
			builder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins");
			builder.Entity<IdentityUserToken<string>>().ToTable("UserTokens");
			builder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims");
			builder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims");
		}
	}
}
