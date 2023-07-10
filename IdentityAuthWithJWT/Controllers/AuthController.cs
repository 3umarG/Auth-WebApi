using IdentityAuthWithJWT.Data;
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

		public AuthController(SignInManager<ApiUser> signInManager, UserManager<ApiUser> userManager)
		{
			_userManager = userManager;
			_signInManager = signInManager;
		}
	}
}
