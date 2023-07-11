using AutoMapper;
using IdentityAuthWithJWT.Data;
using IdentityAuthWithJWT.DTOs;
using IdentityAuthWithJWT.Interfaces;
using IdentityAuthWithJWT.Models;
using IdentityAuthWithJWT.Models.Authentication.Register;
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

		public AuthController(
			IAuthService authService,
			IMapper mapper)
		{
			_authService = authService;
			_mapper = mapper;
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
					_failureFactory = new FailureResponseFactory(400, result.Message);
					return BadRequest(_failureFactory.CreateResponse());
				}

				_successFactory = new SuccessResponseFactory<AuthModel>(200, result);
				return Ok(_successFactory.CreateResponse());
			}
			catch (Exception)
			{
				return Problem("Something went wrong with Login !!");
			}
		}


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
				_successFactory = new SuccessResponseFactory<AddUserToRoleRequestDto>(200, model ,"Added Successfully");
				return Ok(_successFactory.CreateResponse());
			}

			_failureFactory = new FailureResponseFactory(400, "Something went wrong");
			return BadRequest(_failureFactory.CreateResponse());
		}


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
		[HttpGet]
		public  IActionResult GetAllUsersAsync()
		{

			var users = _authService.GetAllUsers();
			_successFactory = new SuccessResponseFactory<List<UserDto>>(200,_mapper.Map<List<UserDto>>(users));
			return Ok(_successFactory.CreateResponse());
		}


		[Authorize(Policy = "RequireAdminAndManagerRoles")]
		[HttpGet("GetAllUsersWithPolicy")]
		public IActionResult GetAllUsersWithPolicyAsync()
		{
			var users = _authService.GetAllUsers();
			_successFactory = new SuccessResponseFactory<List<UserDto>>(200, _mapper.Map<List<UserDto>>(users));
			return Ok(_successFactory.CreateResponse());
		}
	}
}
