using System.Threading;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using Inflow.Services.Customers.Core.DAL;
using Inflow.Services.Customers.Core.DTO;
using Microsoft.EntityFrameworkCore;

namespace Inflow.Services.Customers.Core.Queries.Handlers;

internal sealed class GetCustomerHandler : IQueryHandler<GetCustomer, CustomerDetailsDto>
{
    private readonly CustomersDbContext _dbContext;

    public GetCustomerHandler(CustomersDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<CustomerDetailsDto> HandleAsync(GetCustomer query, CancellationToken cancellationToken = default)
    {
        var customer = await _dbContext.Customers
            .AsNoTracking()
            .SingleOrDefaultAsync(x => x.Id == query.CustomerId);

        return customer?.AsDetailsDto();
    }
}