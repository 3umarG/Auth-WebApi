using IdentityAuthWithJWT.Data;
using IdentityAuthWithJWT.DTOs;
using IdentityAuthWithJWT.Models.Authentication.Register;

namespace IdentityAuthWithJWT.Interfaces
{
	public interface IAuthService
	{
		Task<AuthModel> RegisterAsync(UserDto model, bool isAdmin = false);


		Task<AuthModel> Login(UserLoginDto model);


		Task<string> AddToRoleAsync(AddUserToRoleRequestDto model);

		Task<UpdateUserDto> UpdateUserNameAsync(UpdateUserDto model);

		List<ApiUser> GetAllUsers();

		public Task<AuthModel> RefreshTokenAsync(string oldRefreshToken);
	}
}
