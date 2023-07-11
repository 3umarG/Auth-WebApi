using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace IdentityAuthWithJWT.DTOs
{
	public class UpdateUserDto
	{
		[Required]
        public string OldUserName { get; set; }

		[Required]
		public string NewUserName { get; set; }
    }
}
