using System.Net;

namespace Application.Exceptions
{
    public class InvalidCredentialsException : BaseApplicationException
    {
        public InvalidCredentialsException(string message) : base(message)
        {

        }

        public InvalidCredentialsException(string message, Exception inner) : base(message, inner)
        {

        }
        public override HttpStatusCode StatusCode => HttpStatusCode.Forbidden;

        public override string Title => "Invalid credentials";
    }
}
