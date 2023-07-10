using System.ComponentModel.DataAnnotations;

namespace IdentityAuthWithJWT.DTOs
{
	public class UserDto
	{
		public string FirstName { get; set; }

		public string LastName { get; set; }

		[DataType(DataType.EmailAddress)]
		[Required]
		public string Email { get; set; }


		[DataType(DataType.PhoneNumber)]
		public string PhoneNumber { get; set; }

		[Required]
		[StringLength(15, ErrorMessage = "Your Password should be at least 15 characters")]
		public string Password { get; set; }
	}
}
