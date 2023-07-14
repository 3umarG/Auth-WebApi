using AutoMapper;
using IdentityAuthWithJWT.Data;
using IdentityAuthWithJWT.DTOs;
using IdentityAuthWithJWT.Interfaces;
using IdentityAuthWithJWT.Models.Authentication;
using IdentityAuthWithJWT.Models.Authentication.Register;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace IdentityAuthWithJWT.Models
{
	public class AuthService : IAuthService
	{
		private readonly UserManager<ApiUser> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly IMapper _mapper;
		private readonly JWT _jwt;
		public AuthService(UserManager<ApiUser> userManager, IMapper mapper, IOptions<JWT> jwt, RoleManager<IdentityRole> roleManager)
		{
			_roleManager = roleManager;
			_jwt = jwt.Value;
			_mapper = mapper;
			_userManager = userManager;
		}

		public async Task<string> AddToRoleAsync(AddUserToRoleRequestDto model)
		{
			var user = await _userManager.FindByEmailAsync(model.EmailOrUserName);
			if (user is null)
				return "Invalid User Name/Email";

			if (!await _roleManager.RoleExistsAsync(model.RoleName))
				return "Invalid Role Name";

			if (await _userManager.IsInRoleAsync(user, model.RoleName))
				return $"The User : {model.EmailOrUserName} is already assigned to : {model.RoleName} Role";

			var result = await _userManager.AddToRoleAsync(user, model.RoleName);
			if (!result.Succeeded)
				return "Something went wrong !!";

			return string.Empty;
		}

		public async Task<AuthModel> Login(UserLoginDto model)
		{
			var auth = new AuthModel();

			var user = await _userManager.FindByEmailAsync(model.Email);

			if (user is null || !await _userManager.CheckPasswordAsync(user, model.Password))
			{
				auth.Message = "Your Email or Password is not correct";
				return auth;
			}

			// get the token:
			var jwtSecurityToken = await CreateJwtToken(user);

			auth.Email = user.Email;
			auth.UserName = user.UserName;
			auth.AccessTokenExpiration = jwtSecurityToken.ValidTo;
			auth.IsAuthed = true;
			auth.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

			// in this case i ask for any active refresh token that still valid and assign to user
			// I can do another scenario by assign new refresh token to user if he login and revoke any active token
			// ... I can do that by remove the if() part and only generate new one directly , and add Revoke() method 
			if (user.RefreshTokens.Any(t => t.IsActive))
			{
				var activeRefreshToken = user.RefreshTokens.FirstOrDefault(t => t.IsActive);
				auth.RefreshToken = activeRefreshToken.Token;
				auth.RefreshTokenExpiration = activeRefreshToken.ExpiresOn;
			}
			else
			{
				var generatedRefreshToken = GenerateRefreshToken();

				auth.RefreshToken = generatedRefreshToken.Token;
				auth.RefreshTokenExpiration = generatedRefreshToken.ExpiresOn;

				user.RefreshTokens.Add(generatedRefreshToken);
				await _userManager.UpdateAsync(user);
			}

			return auth;
		}

		public async Task<AuthModel> RegisterAsync(UserDto model, bool isAdmin = false)
		{
			var usedEmail = await _userManager.FindByEmailAsync(model.Email);
			if (usedEmail is not null)
			{
				return new AuthModel
				{
					Message = "The Provided Email already used by another user"
				};
			}

			var apiUser = _mapper.Map<ApiUser>(model);
			apiUser.UserName = model.Email;

			var result = await _userManager.CreateAsync(apiUser, model.Password);

			if (!result.Succeeded)
			{
				var errors = new List<string>();
				foreach (var error in result.Errors)
				{
					errors.Add(error.Description);
				}
				return new AuthModel
				{
					Message = "Something Went Wrong when register !!",
					Errors = errors,
				};
			}

			if (isAdmin)
				await _userManager.AddToRoleAsync(apiUser, "Admin");
			else
				await _userManager.AddToRoleAsync(apiUser, "User");


			// get the token:
			var jwtSecurityToken = await CreateJwtToken(apiUser);

			return new AuthModel
			{
				Email = apiUser.Email,
				IsAuthed = true,
				AccessTokenExpiration = jwtSecurityToken.ValidTo,
				Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
				UserName = apiUser.Email
			};

		}

		public async Task<UpdateUserDto> UpdateUserNameAsync(UpdateUserDto model)
		{
			var user = await _userManager.FindByNameAsync(model.OldUserName)
				?? throw new Exception("There is no User with that Name");

			var userWithNewUserName = await _userManager.FindByNameAsync(model.NewUserName);

			if (userWithNewUserName is not null)
				throw new Exception("There is already existed User with the new username");

			user.UserName = model.NewUserName;
			user.Email = model.NewUserName;

			await _userManager.UpdateAsync(user);
			return model;
		}

		private async Task<JwtSecurityToken> CreateJwtToken(ApiUser user)
		{
			var userClaims = await _userManager.GetClaimsAsync(user);
			var roles = await _userManager.GetRolesAsync(user);
			var roleClaims = new List<Claim>();

			foreach (var role in roles)
				roleClaims.Add(new Claim("roles", role));

			var claims = new[]
			{
				new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
				new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
				new Claim(JwtRegisteredClaimNames.Email, user.Email),
				new Claim("uid", user.Id)
			}
			.Union(userClaims)
			.Union(roleClaims);

			var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
			var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

			var jwtSecurityToken = new JwtSecurityToken(
				issuer: _jwt.Issuer,
				audience: _jwt.Audience,
				claims: claims,
				expires: DateTime.Now.AddMinutes(_jwt.DurationInMinutes),
				signingCredentials: signingCredentials);

			return jwtSecurityToken;
		}

		private RefreshToken GenerateRefreshToken()
		{
			var randomNumber = new byte[32];

			using var generator = new RNGCryptoServiceProvider();

			generator.GetBytes(randomNumber);

			return new RefreshToken
			{
				Token = Convert.ToBase64String(randomNumber),
				ExpiresOn = DateTime.UtcNow.AddDays(10),
				CreatedOn = DateTime.UtcNow
			};
		}

		public List<ApiUser> GetAllUsers()
		{
			var users = _userManager.Users.ToList();
			return users;
		}
	}


}

