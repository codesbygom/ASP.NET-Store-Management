using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Store.Application.Contracts.Persistence;
using Store.Application.DTOs.Customer;
using Store.Application.Exceptions;
using Store.Application.Features.Interfaces;

namespace Store.Application.Features.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;

        public CustomerService(ICustomerRepository customerRepository, IMapper mapper)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
        }

        public async Task<List<CustomerDTO>> GetAll()
        {
            var items = await _customerRepository.GetAll();

            return _mapper.Map<List<CustomerDTO>>(items);
        }

        public async Task<CustomerDTO> Get(int id)
        {
            var find = await _customerRepository.GetNoTracking(id);

            if (find == null)
            {
                throw new NotFoundException("Customer not found");
            }

            return _mapper.Map<CustomerDTO>(find);
        }

        public async Task<CustomerDTO> Edit(UpsertCustomer input)
        {
            var model = await _customerRepository.Get(input.Id.GetValueOrDefault());

            if (model == null)
            {
                throw new NotFoundException("Customer not found");
            }

            var sameEmail = await _customerRepository.GetAllQueryable()
                .AnyAsync(a => a.Email == input.Email && a.Id != model.Id);

            if (sameEmail)
            {
                throw new BadRequestException("This email is already registered");
            }

            _mapper.Map(input, model);

            await _customerRepository.Update(model);

            return _mapper.Map<CustomerDTO>(model);
        }

        public async Task<bool> Delete(int id)
        {
            await _customerRepository.Delete(id);

            return true;
        }

        public async Task<bool> Recover(int id)
        {
            await _customerRepository.Recover(id);

            return true;
        }
    }
}
