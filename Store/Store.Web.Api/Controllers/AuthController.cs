using Microsoft.AspNetCore.Mvc;
using Store.Application.DTOs.Authentication;
using Store.Application.Features.Interfaces;
using Store.Application.Responses;

namespace Store.Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost("register")]
        [Produces(typeof(Response<AuthResult>))]
        public async Task<JsonResult> Register(Register input)
        {
            var res = await _authenticationService.Register(input);

            return new Response<AuthResult>(res).ToJsonResult();
        }

        [HttpPost("login")]
        [Produces(typeof(Response<AuthResult>))]
        public async Task<JsonResult> Login(Authenticate input)
        {
            var res = await _authenticationService.Login(input);

            return new Response<AuthResult>(res).ToJsonResult();
        }
    }
}
