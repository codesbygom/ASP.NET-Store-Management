using Store.Application.DTOs.Authentication;

namespace Store.Application.Features.Interfaces
{
    public interface IAuthenticationService
    {
        Task<AuthResult> Register(Register input);
        Task<AuthResult> Login(Authenticate input);
    }
}
