namespace IdentityAuthWithJWT.Models
{
	public class JWT
	{
		//"JWT": {
		//  "Key": "sz8eI7OdHBrjrIo8j9nTW/rQyO1OvY0pAQ2wDKQZw/0=",
		//  "Issuer": "SecureApi",
		//  "Audience": "SecureApiUser",
		//  "DurationInDays": 30
		//}

		public string Key { get; set; }

		public string Issuer { get; set; }
		
		public string Audience { get; set; }
		
		public string DurationInDays { get; set; }
	}
}
