using IdentityAuthWithJWT.Interfaces;
using System.Text.Json;

namespace IdentityAuthWithJWT.Models
{
	public class ForbiddenFailureResponse : IResponse
	{
		public int StatusCode { get; private set; }

		public string? Message { get; private set; }

		public bool Status { get; private set; }

        public ForbiddenFailureResponse()
        {
			StatusCode = 403;
			Message = "You are Forbidden from accessing this end point , this end point need certain Role";
			Status = false;
        }

		public override string ToString()
		{
			var options = new JsonSerializerOptions
			{
				PropertyNamingPolicy = JsonNamingPolicy.CamelCase
			};
			return JsonSerializer.Serialize(this, options);
		}
	}
}
