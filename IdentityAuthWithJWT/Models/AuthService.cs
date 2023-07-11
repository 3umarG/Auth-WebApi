using AutoMapper;
using IdentityAuthWithJWT.Data;
using IdentityAuthWithJWT.DTOs;
using IdentityAuthWithJWT.Interfaces;
using IdentityAuthWithJWT.Models.Authentication.Register;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace IdentityAuthWithJWT.Models
{
	public class AuthService : IAuthService
	{
		private readonly UserManager<ApiUser> _userManager;
		private readonly IMapper _mapper;
		private readonly JWT _jwt;
		public AuthService(UserManager<ApiUser> userManager, IMapper mapper, IOptions<JWT> jwt)
		{
			_jwt = jwt.Value;
			_mapper = mapper;
			_userManager = userManager;
		}

		public async Task<AuthModel> Login(UserLoginDto model)
		{
			var auth = new AuthModel();

			var user = await _userManager.FindByEmailAsync(model.Email);

			if(user is null || !await _userManager.CheckPasswordAsync(user , model.Password))
			{
				auth.Message = "Your Email or Password is not correct";
				return auth;
			}

			// get the token:
			var jwtSecurityToken = await CreateJwtToken(user);

			auth.Email = user.Email;
			auth.UserName = user.UserName;
			auth.ExpiresOn = jwtSecurityToken.ValidTo;
			auth.IsAuthed = true;
			auth.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

			return auth;
		}

		public async Task<AuthModel> RegisterAsync(UserDto model)
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

			await _userManager.AddToRoleAsync(apiUser, "User");

			// get the token:
			var jwtSecurityToken = await CreateJwtToken(apiUser);

			return new AuthModel
			{
				Email = apiUser.Email,
				IsAuthed = true,
				ExpiresOn = jwtSecurityToken.ValidTo,
				Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
				UserName = apiUser.Email
			};

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
				expires: DateTime.Now.AddDays(_jwt.DurationInDays),
				signingCredentials: signingCredentials);

			return jwtSecurityToken;
		}
	}


}

