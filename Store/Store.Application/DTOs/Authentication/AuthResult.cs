using Store.Application.DTOs.Customer;

namespace Store.Application.DTOs.Authentication
{
    public class AuthResult
    {
        public string Token { get; set; }
        public CustomerDTO Customer { get; set; }
    }
}
