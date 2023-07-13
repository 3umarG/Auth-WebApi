using IdentityAuthWithJWT.Interfaces;

namespace IdentityAuthWithJWT.Models
{
	public class UnAuthorizedFailureResponse : IResponse
	{
		public int StatusCode { get; private set; }

		public string? Message { get; private set; }

		public bool Status { get; private set; }


        public UnAuthorizedFailureResponse()
        {
            Status = false;
			StatusCode = 401;
			Message = "You are UnAuthorized , please provide correct token to your headers";
        }
    }
}
