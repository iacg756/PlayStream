using System.Net;
namespace PlayStream.Core.Exceptions
{
    public class BussinesException : Exception
    {
        public HttpStatusCode StatusCode { get; }
        public object? Details { get; }

        public BussinesException(string message,
            HttpStatusCode statusCode = HttpStatusCode.BadRequest,
            object? details = null) : base(message)
        {
            StatusCode = statusCode;
            Details = details;
        }
    }
}