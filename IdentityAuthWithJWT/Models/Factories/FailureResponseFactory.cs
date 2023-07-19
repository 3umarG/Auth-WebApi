using IdentityAuthWithJWT.Interfaces;
using IdentityAuthWithJWT.Models.Responses;

namespace IdentityAuthWithJWT.Models.Factories
{
    public class FailureResponseFactory : IResponseFactory
    {
        private readonly FailureResponse _failureResponse;

        public FailureResponseFactory(int statusCode, string? message = null)
        {
            _failureResponse = new FailureResponse(statusCode, message);
        }
        public IResponse CreateResponse()
        {
            return _failureResponse;
        }
    }
}
