using IdentityAuthWithJWT.Models.Authentication;
using Microsoft.AspNetCore.Identity;

namespace IdentityAuthWithJWT.Data
{
	public class ApiUser : IdentityUser
	{
		public string FirstName { get; set; }

		public string LastName { get; set; }

        public ICollection<RefreshToken>? RefreshTokens { get; set; }
    }
}
