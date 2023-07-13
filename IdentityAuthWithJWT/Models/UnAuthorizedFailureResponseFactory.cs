using IdentityAuthWithJWT.Interfaces;

namespace IdentityAuthWithJWT.Models
{
	public class UnAuthorizedFailureResponseFactory : IResponseFactory
	{
		private readonly UnAuthorizedFailureResponse _response;

        public UnAuthorizedFailureResponseFactory()
        {
            _response = new UnAuthorizedFailureResponse();
        }
        public IResponse CreateResponse()
		{
			return _response;
		}
	}
}
