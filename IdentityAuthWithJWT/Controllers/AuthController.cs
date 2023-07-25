using AutoMapper;
using IdentityAuthWithJWT.Data;
using IdentityAuthWithJWT.DTOs;
using IdentityAuthWithJWT.Interfaces;
using IdentityAuthWithJWT.Models;
using IdentityAuthWithJWT.Models.Authentication.Register;
using IdentityAuthWithJWT.Models.Factories;
using IdentityAuthWithJWT.Models.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace IdentityAuthWithJWT.Controllers
{
    [Route("api/[controller]")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		//private readonly UserManager<ApiUser> _userManager;
		private readonly IMapper _mapper;
		private readonly IAuthService _authService;
		private IResponseFactory _successFactory;
		private IResponseFactory _failureFactory;
		private readonly IResponseFactory _unAuthorizedFactory;
		private readonly IResponseFactory _forbiddenFactory;

		public AuthController(
			IAuthService authService,
			IMapper mapper)
		{
			_authService = authService;
			_mapper = mapper;
			_unAuthorizedFactory = new UnAuthorizedFailureResponseFactory();
			_forbiddenFactory = new ForbiddenFailureResponseFactory();
		}


		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SuccessResponse<AuthModel>))]
		[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(FailureResponse))]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
					_failureFactory = new FailureResponseFactory(400, result.Message);
					return BadRequest(_failureFactory.CreateResponse());
				}


				_successFactory = new SuccessResponseFactory<AuthModel>(200, result);
				return Ok(_successFactory.CreateResponse());
			}
			catch (Exception)
			{
				return Problem("Something went wrong , please try again later !!", statusCode: 500);
			}
		}


		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SuccessResponse<AuthModel>))]
		[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(FailureResponse))]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		[HttpPost("RegisterAdmin")]
		public async Task<IActionResult> RegisterAdmin([FromBody] UserDto userDto)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			try
			{
				var result = await _authService.RegisterAsync(userDto, true);
				if (!result.IsAuthed)
				{
					_failureFactory = new FailureResponseFactory(400, result.Message);
					return BadRequest(_failureFactory.CreateResponse());
				}



				_successFactory = new SuccessResponseFactory<AuthModel>(200, result);
				return Ok(_successFactory.CreateResponse());
			}
			catch (Exception)
			{
				return Problem("Something went wrong , please try again later !!", statusCode: 500);
			}
		}


		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SuccessResponse<AuthModel>))]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(UnAuthorizedFailureResponse))]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		[HttpPost("Login")]
		public async Task<IActionResult> Login([FromBody] UserLoginDto userDto)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			try
			{
				var result = await _authService.Login(userDto);

				if (!result.IsAuthed)
				{
					return new UnauthorizedObjectResult(_unAuthorizedFactory.CreateResponse());
				}

				_successFactory = new SuccessResponseFactory<AuthModel>(200, result);

				if (!result.RefreshToken.IsNullOrEmpty())
				{
					SetRefreshTokenToCookies(result.RefreshToken, result.RefreshTokenExpiration);
				}
				return Ok(_successFactory.CreateResponse());
			}
			catch (Exception)
			{
				return Problem("Something went wrong with Login !!");
			}
		}



		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SuccessResponse<AddUserToRoleRequestDto>))]
		[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(FailureResponse))]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		[HttpPost("AddUserToRole")]
		public async Task<IActionResult> AddToRole([FromBody] AddUserToRoleRequestDto model)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var result = await _authService.AddToRoleAsync(model);

			if (result.IsNullOrEmpty())
			{
				_successFactory = new SuccessResponseFactory<AddUserToRoleRequestDto>(200, model, "Added Successfully");
				return Ok(_successFactory.CreateResponse());
			}

			_failureFactory = new FailureResponseFactory(400, "Something went wrong");
			return BadRequest(_failureFactory.CreateResponse());
		}



		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SuccessResponse<UpdateUserDto>))]
		[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(FailureResponse))]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		[HttpPost("Update")]
		public async Task<IActionResult> Update([FromBody] UpdateUserDto newUser)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			try
			{
				var updatedUser = await _authService.UpdateUserNameAsync(newUser);
				_successFactory = new SuccessResponseFactory<UpdateUserDto>(200, updatedUser);
				return Ok(_successFactory.CreateResponse());
			}
			catch (Exception ex)
			{
				_failureFactory = new FailureResponseFactory(400, ex.Message);
				return BadRequest(_failureFactory.CreateResponse());
			}
		}




		//[Authorize]     // for forcing the user/client to send token for it
		[Authorize(Roles = "Admin")]   // for determine specific rule to use this end point
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SuccessResponse<List<UserDto>>))]
		[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(FailureResponse))]
		[ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(UnAuthorizedFailureResponse))]
		[ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(ForbiddenFailureResponse))]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		[HttpGet]
		public IActionResult GetAllUsersAsync()
		{

			var users = _authService.GetAllUsers();
			_successFactory = new SuccessResponseFactory<List<UserDto>>(200, _mapper.Map<List<UserDto>>(users));
			return Ok(_successFactory.CreateResponse());
		}



		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SuccessResponse<List<UserDto>>))]
		[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(FailureResponse))]
		[ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(UnAuthorizedFailureResponse))]
		[ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(ForbiddenFailureResponse))]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		[Authorize(Policy = "RequireAdminAndManagerRoles")]
		[HttpGet("GetAllUsersWithPolicy")]
		public IActionResult GetAllUsersWithPolicyAsync()
		{
			var users = _authService.GetAllUsers();
			_successFactory = new SuccessResponseFactory<List<UserDto>>(200, _mapper.Map<List<UserDto>>(users));
			return Ok(_successFactory.CreateResponse());
		}


		[HttpGet("RefreshToken")]
		public async Task<IActionResult> RefreshTokenAsync()
		{
			var refreshToken = Request.Cookies["refreshToken"];

			if (refreshToken is null)
				return BadRequest("Please send RefreshToken by Cookies");

			var auth = await _authService.RefreshTokenAsync(refreshToken);

			if (!auth.IsAuthed)
			{
				return BadRequest(auth.Message);
			}

			SetRefreshTokenToCookies(auth.RefreshToken, auth.RefreshTokenExpiration);
			
			_successFactory = new SuccessResponseFactory<AuthModel>(200, auth);
			return Ok(_successFactory.CreateResponse());
		}

		[HttpPost("RevokeToken")]
		public async Task<IActionResult> RevokeTokenAsync()
		{
			var token = Request.Cookies["refreshToken"];

			if (token is null)
				return BadRequest("Refresh Token is Required");

			var result = await _authService.RevokeTokenAsync(token);

			if (!result)
				return BadRequest("Invalid Refresh Token");

			return Ok("Revoked");
		}



		private void SetRefreshTokenToCookies(string refreshToken, DateTime expiresOn)
		{
			var cookieOptions = new CookieOptions
			{
				HttpOnly = true,
				Expires = expiresOn.ToLocalTime()
			};

			Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
		}
	}
}
