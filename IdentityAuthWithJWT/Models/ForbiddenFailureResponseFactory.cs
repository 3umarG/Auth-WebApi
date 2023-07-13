using IdentityAuthWithJWT.Interfaces;

namespace IdentityAuthWithJWT.Models
{
	public class ForbiddenFailureResponseFactory : IResponseFactory
	{
		private readonly ForbiddenFailureResponse _response;

        public ForbiddenFailureResponseFactory()
        {
            _response = new ForbiddenFailureResponse();
        }

		public IResponse CreateResponse()
		{
			return _response;
		}
	}
}
