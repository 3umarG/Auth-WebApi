using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace IdentityAuthWithJWT.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize]
	public class SecuredController : ControllerBase
	{

		[HttpGet]
		public IActionResult Get()
		{
			string token = Request.Headers.Authorization.First().Split(" ")[1];
			string email = extractUserInfoFromJwtToken(token , "uid");
			return Ok(email);
		}

		private static string extractUserInfoFromJwtToken(string token , string info)
		{
			var handler = new JwtSecurityTokenHandler();
			var jwtSecurityToken = handler.ReadJwtToken(token);

			var email = jwtSecurityToken.Claims.First(claim => claim.Type == info).Value;
			return email;
		}
	}
}
