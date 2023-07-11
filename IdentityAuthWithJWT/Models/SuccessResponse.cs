using IdentityAuthWithJWT.Interfaces;

namespace IdentityAuthWithJWT.Models
{
	public class SuccessResponse<T> : IResponse where T : class
	{
		public int StatusCode { get; private set; }
		public string? Message { get; private set; }
		public bool Status { get; private set; }

		public T Data { get; private set; }

        public SuccessResponse(int statusCode , T data , string? message = null)
        {
			Message = message;
			Status = true;
			StatusCode = statusCode;
			Data = data;

        }
    }
}
