namespace IdentityAuthWithJWT.Models.Authentication
{
	public class MailSettings
	{
		/*
		 "Mail": "omardeveloper242@gmail.com",
		"Password": "fixcmojopqwoglsl",
		"Host": "smtp.gmail.com",
		"Port": 587, // TLS,
		"DisplayName": "OmarGomaa" 
		 */

		public string Mail { get; set; }

		public string Password { get; set; }

		public string Host { get; set; }

		public int Port { get; set; }

		public string DisplayName { get; set; }

	}
}
