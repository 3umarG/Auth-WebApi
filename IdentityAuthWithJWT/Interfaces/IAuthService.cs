using IdentityAuthWithJWT.DTOs;
using IdentityAuthWithJWT.Models.Authentication.Register;

namespace IdentityAuthWithJWT.Interfaces
{
	public interface IAuthService
	{
		Task<AuthModel> RegisterAsync(UserDto model);

		Task<AuthModel> Login(UserLoginDto model);
	}
}
