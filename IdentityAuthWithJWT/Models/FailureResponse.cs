using IdentityAuthWithJWT.Interfaces;
using System.Text.Json;

namespace IdentityAuthWithJWT.Models
{
	public class FailureResponse : IResponse
	{
		public int StatusCode { get; private set; }

		public string? Message { get; private set; }

		public bool Status { get; private set; }


		public FailureResponse(int statusCode, string? message = "Failed")
		{
			StatusCode = statusCode;
			Message = message;
			Status = false;
		}

		public override string ToString()
		{
			return JsonSerializer.Serialize(this);
		}
	}
}
