using IdentityAuthWithJWT.Interfaces;

namespace IdentityAuthWithJWT.Models
{
	public class FailureResponseFactory : IResponseFactory
	{
		private readonly FailureResponse _failureResponse;

        public FailureResponseFactory(int statusCode , string? message = null)
        {
            _failureResponse = new FailureResponse(statusCode, message);
        }
        public IResponse CreateResponse()
		{
			return _failureResponse;
		}
	}
}
