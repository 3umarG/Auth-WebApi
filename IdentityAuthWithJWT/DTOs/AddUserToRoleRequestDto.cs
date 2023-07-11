using System.ComponentModel.DataAnnotations;

namespace IdentityAuthWithJWT.DTOs
{
	public class AddUserToRoleRequestDto
	{
        [Required]
        public string EmailOrUserName { get; set; }
		
		
		[Required]
		public string RoleName { get; set; }
    }
}
