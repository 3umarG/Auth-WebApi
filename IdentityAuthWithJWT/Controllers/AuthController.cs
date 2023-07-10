using AutoMapper;
using IdentityAuthWithJWT.Data;
using IdentityAuthWithJWT.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityAuthWithJWT.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		private readonly UserManager<ApiUser> _userManager;
		private readonly SignInManager<ApiUser> _signInManager;
		private readonly IMapper _mapper;

		public AuthController(
			SignInManager<ApiUser> signInManager,
			UserManager<ApiUser> userManager,
			IMapper mapper)
		{
			_mapper = mapper;
			_userManager = userManager;
			_signInManager = signInManager;
		}

		[HttpPost("Register")]
		public async Task<IActionResult> Register([FromBody] UserDto userDto)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			try
			{
				var userApi = _mapper.Map<ApiUser>(userDto);
				userApi.UserName = userDto.Email;
				var result = await _userManager.CreateAsync(userApi, userDto.Password);

				if (!result.Succeeded)
				{
					foreach (var error in result.Errors)
					{
						ModelState.AddModelError(error.Code, error.Description);
					}
					return BadRequest(ModelState);
				}

				return Ok(userDto);
			}
			catch (Exception)
			{
				return Problem("Something went wrong , please try again later !!", statusCode: 500);
			}
		}


		[HttpPost("Login")]
		public async Task<IActionResult> Login([FromBody] UserLoginDto userDto)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			try
			{
				var result = await _signInManager.PasswordSignInAsync(userDto.Email, userDto.Password ,false , false);
				if (!result.Succeeded)
				{
					return Unauthorized();
                }

				return Ok(userDto);
			}
			catch (Exception)
			{
				return Problem("Something went wrong with Login !!");
			}
		}
	}
}
