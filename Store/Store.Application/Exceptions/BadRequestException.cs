namespace Store.Application.Exceptions
{
    public class BadRequestException : ApplicationException
    {
        public BadRequestException(string message) : base(message)
        {
        }

        public BadRequestException() : base("Failed to save the information")
        {
        }
    }
}
