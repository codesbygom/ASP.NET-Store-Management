using AutoMapper;
using Store.Application.Contracts.Infrastructure;
using Store.Application.Contracts.Persistence;
using Store.Application.DTOs.Authentication;
using Store.Application.DTOs.Customer;
using Store.Application.Exceptions;
using Store.Application.Features.Interfaces;
using Store.Domain.Entities;

namespace Store.Application.Features.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IMapper _mapper;

        public AuthenticationService(
            ICustomerRepository customerRepository,
            IPasswordHasher passwordHasher,
            IJwtTokenService jwtTokenService,
            IMapper mapper)
        {
            _customerRepository = customerRepository;
            _passwordHasher = passwordHasher;
            _jwtTokenService = jwtTokenService;
            _mapper = mapper;
        }

        public async Task<AuthResult> Register(Register input)
        {
            if (await _customerRepository.GetByEmail(input.Email) != null)
            {
                throw new BadRequestException("This email is already registered");
            }

            var customer = _mapper.Map<Customer>(input);
            customer.PasswordHash = _passwordHasher.Hash(input.Password);

            customer = await _customerRepository.Add(customer);

            return new AuthResult
            {
                Token = _jwtTokenService.Generate(customer),
                Customer = _mapper.Map<CustomerDTO>(customer)
            };
        }

        public async Task<AuthResult> Login(Authenticate input)
        {
            var customer = await _customerRepository.GetByEmail(input.Email);

            if (customer == null || !customer.IsActive || !_passwordHasher.Verify(input.Password, customer.PasswordHash))
            {
                throw new UnauthorizedAccessException("Invalid email or password");
            }

            return new AuthResult
            {
                Token = _jwtTokenService.Generate(customer),
                Customer = _mapper.Map<CustomerDTO>(customer)
            };
        }
    }
}
