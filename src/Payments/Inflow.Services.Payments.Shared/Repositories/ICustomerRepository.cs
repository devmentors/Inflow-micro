using System;
using System.Threading.Tasks;
using Inflow.Services.Payments.Shared.Entities;

namespace Inflow.Services.Payments.Shared.Repositories;

internal interface ICustomerRepository
{
    Task<Customer> GetAsync(Guid id);
    Task AddAsync(Customer customer);
    Task UpdateAsync(Customer customer);
}