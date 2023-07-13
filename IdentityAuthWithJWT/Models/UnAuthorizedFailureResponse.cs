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
			StatusCode = 403;
			Message = "You are UnAuthorized from accessing this end point , this end point need certain Role";
			Status = false;
        }
    }
}
