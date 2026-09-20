using Store.Application.DTOs.Base;

namespace Store.Application.DTOs.Customer
{
    public class CustomerDTO : BaseDTO
    {
        public string Email { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Nickname { get; set; } = string.Empty;
    }
}
