using System;
using System.Threading.Tasks;
using Inflow.Services.Payments.Shared.Entities;
using Inflow.Services.Payments.Shared.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Inflow.Services.Payments.Core.DAL.Repositories;

internal class CustomerRepository : ICustomerRepository
{
    private readonly PaymentsDbContext _context;
    private readonly DbSet<Customer> _customers;

    public CustomerRepository(PaymentsDbContext context)
    {
        _context = context;
        _customers = _context.Customers;
    }

    public  Task<Customer> GetAsync(Guid id)
        => _customers.SingleOrDefaultAsync(x => x.Id == id);

    public async Task AddAsync(Customer customer)
    {
        await _customers.AddAsync(customer);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Customer customer)
    {
        _customers.Update(customer);
        await _context.SaveChangesAsync();
    }
}