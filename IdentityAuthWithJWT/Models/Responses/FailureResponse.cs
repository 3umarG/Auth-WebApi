using IdentityAuthWithJWT.Interfaces;
using System.Text.Json;

namespace IdentityAuthWithJWT.Models.Responses
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
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            return JsonSerializer.Serialize(this, options);
        }
    }
}
