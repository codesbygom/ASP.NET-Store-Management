namespace Store.Application.Exceptions
{
    public class NotFoundException : ApplicationException
    {
        public NotFoundException(string item) : base(item)
        {
        }

        public NotFoundException() : base("The requested item was not found")
        {
        }
    }
}
