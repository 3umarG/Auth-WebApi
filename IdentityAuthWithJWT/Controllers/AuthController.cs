using AutoMapper;
using IdentityAuthWithJWT.Data;
using IdentityAuthWithJWT.DTOs;
using IdentityAuthWithJWT.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityAuthWithJWT.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		//private readonly UserManager<ApiUser> _userManager;
		private readonly SignInManager<ApiUser> _signInManager;
		private readonly IMapper _mapper;
		private readonly IAuthService _authService;

		public AuthController(
			SignInManager<ApiUser> signInManager,
			IAuthService authService,
			IMapper mapper)
		{
			_authService = authService;
			_mapper = mapper;
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
				var result = await _authService.RegisterAsync(userDto);
				if (!result.IsAuthed)
				{
					return BadRequest(result.Message);
				}


			
				return Ok(result);
			}
			catch (Exception)
			{
				return Problem("Something went wrong , please try again later !!", statusCode: 500);
			}
		}

		/*
		[HttpPost("RegisterAdmin")]
		public async Task<IActionResult> AdminRegister([FromBody] UserDto userDto)
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

				await _userManager.AddToRoleAsync(userApi, "Admin");
				return Ok(userDto);
			}
			catch (Exception)
			{
				return Problem("Something went wrong , please try again later !!", statusCode: 500);
			}
		}
		*/

		[HttpPost("Login")]
		public async Task<IActionResult> Login([FromBody] UserLoginDto userDto)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			try
			{
				var result = await _signInManager.PasswordSignInAsync(userDto.Email, userDto.Password, false, false);
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

		/*
		[HttpPost("Update")]
		public async Task<IActionResult> Update( [FromBody] UpdateUserDto newUser)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			try
			{
				var user = await _userManager.FindByEmailAsync(newUser.Email);

				if(user is null)
				{
					return NotFound("There is no User with that ID");
				}

				user.UserName = newUser.UserName;
				user.Email = newUser.UserName;

				await _userManager.UpdateAsync(user);
				return Ok(user);
			}
			catch (Exception)
			{
				return Problem("There is an error occured",statusCode: 500);
			}
		}
		*/
	}
}
