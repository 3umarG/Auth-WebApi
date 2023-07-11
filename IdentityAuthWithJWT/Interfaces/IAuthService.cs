using IdentityAuthWithJWT.DTOs;
using IdentityAuthWithJWT.Models.Authentication.Register;

namespace IdentityAuthWithJWT.Interfaces
{
	public interface IAuthService
	{
		Task<AuthModel> RegisterAsync(UserDto model , bool isAdmin = false);

		Task<AuthModel> Login(UserLoginDto model);
	}
}
