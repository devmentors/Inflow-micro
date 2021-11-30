using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using Inflow.Services.Customers.Core.DAL;
using Inflow.Services.Customers.Core.DTO;
using Microsoft.EntityFrameworkCore;

namespace Inflow.Services.Customers.Core.Queries.Handlers;

internal sealed class BrowseCustomersHandler : IQueryHandler<BrowseCustomers, PagedResult<CustomerDto>>
{
    private readonly CustomersDbContext _dbContext;

    public BrowseCustomersHandler(CustomersDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<PagedResult<CustomerDto>> HandleAsync(BrowseCustomers query,
        CancellationToken cancellationToken = default)
    {
        var customers = _dbContext.Customers.AsQueryable();
        if (!string.IsNullOrWhiteSpace(query.State))
        {
            var state = query.State.ToLowerInvariant();
            customers = state switch
            {
                "new" => customers.Where(x => !x.CompletedAt.HasValue && !x.VerifiedAt.HasValue),
                "completed" => customers.Where(x => x.CompletedAt.HasValue),
                "verified" => customers.Where(x => x.VerifiedAt.HasValue),
                "locked" => customers.Where(x => !x.IsActive),
                _ => customers
            };
        }

        return customers.AsNoTracking()
            .OrderByDescending(x => x.CreatedAt)
            .Select(x => x.AsDto())
            .PaginateAsync(query, cancellationToken);
    }
}