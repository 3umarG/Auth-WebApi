using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
			return Ok("Get Successfully");
		}
	}
}
