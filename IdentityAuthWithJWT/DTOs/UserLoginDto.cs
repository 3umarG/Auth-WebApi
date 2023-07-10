using System.ComponentModel.DataAnnotations;

namespace IdentityAuthWithJWT.DTOs
{
	public class UserLoginDto
	{
		[DataType(DataType.EmailAddress)]
		[Required]
		public string Email { get; set; }


		[Required]
		[StringLength(15, ErrorMessage = "Your Password should be at least 15 characters")]
		public string Password { get; set; }
	}
}
