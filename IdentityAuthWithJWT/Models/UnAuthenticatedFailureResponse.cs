using IdentityAuthWithJWT.Interfaces;

namespace IdentityAuthWithJWT.Models
{
	public class UnAuthenticatedFailureResponse : IResponse
	{
		public int StatusCode { get; private set; }

		public string? Message { get; private set; }

		public bool Status { get; private set; }


        public UnAuthenticatedFailureResponse()
        {
            Status = false;
			StatusCode = 401;
			Message = "You are UnAuthenticated , please provide correct token to your headers";
        }
    }
}
