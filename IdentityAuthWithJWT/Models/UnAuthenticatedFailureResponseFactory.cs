using IdentityAuthWithJWT.Interfaces;

namespace IdentityAuthWithJWT.Models
{
	public class UnAuthenticatedFailureResponseFactory : IResponseFactory
	{
		private readonly UnAuthenticatedFailureResponse _response;

        public UnAuthenticatedFailureResponseFactory()
        {
            _response = new UnAuthenticatedFailureResponse();
        }
        public IResponse CreateResponse()
		{
			return _response;
		}
	}
}
