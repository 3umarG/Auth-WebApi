using IdentityAuthWithJWT.Interfaces;
using IdentityAuthWithJWT.Models.Responses;

namespace IdentityAuthWithJWT.Models.Factories
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
