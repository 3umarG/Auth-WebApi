using IdentityAuthWithJWT.Interfaces;
using IdentityAuthWithJWT.Models.Responses;

namespace IdentityAuthWithJWT.Models.Factories
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
