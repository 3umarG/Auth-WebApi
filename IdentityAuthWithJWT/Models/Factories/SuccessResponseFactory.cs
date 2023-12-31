﻿using IdentityAuthWithJWT.Interfaces;
using IdentityAuthWithJWT.Models.Responses;

namespace IdentityAuthWithJWT.Models.Factories
{
    public class SuccessResponseFactory<T> : IResponseFactory where T : class
    {
        private readonly SuccessResponse<T> _successResponse;
        public SuccessResponseFactory(int statusCode, T data, string? message = null)
        {
            _successResponse = new SuccessResponse<T>(statusCode, data, message);
        }
        public IResponse CreateResponse()
        {
            return _successResponse;
        }
    }
}
