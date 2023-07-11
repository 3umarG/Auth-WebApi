using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace IdentityAuthWithJWT.DTOs
{
	public class UpdateUserDto
	{
		[Required]
		[DataType(DataType.EmailAddress)]
        public string Email { get; set; }

		[Required]
		public string UserName { get; set; }
    }
}
