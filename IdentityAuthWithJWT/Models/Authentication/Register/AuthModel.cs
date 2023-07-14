using System.Text.Json.Serialization;

namespace IdentityAuthWithJWT.Models.Authentication.Register
{
    /// That holds the information/result of any Authentication method
	public class AuthModel
	{
        public string Email { get; set; }

        public string UserName { get; set; }

        public string Message { get; set; }

        public List<string> Errors { get; set; }

        public string Token { get; set; }

        public DateTime AccessTokenExpiration { get; set; }

        [JsonIgnore]
        public string? RefreshToken { get; set; }

        public DateTime RefreshTokenExpiration { get; set; }

        public bool IsAuthed { get; set; }
    }
}
