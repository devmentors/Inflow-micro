using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using Inflow.Services.Payments.Core.DAL;
using Inflow.Services.Payments.Core.Withdrawals.DTO;
using Inflow.Services.Payments.Shared.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Inflow.Services.Payments.Core.Withdrawals.Queries.Handlers;

internal sealed class BrowseWithdrawalAccountsHandler : IQueryHandler<BrowseWithdrawalAccounts, PagedResult<WithdrawalAccountDto>>
{
    private readonly PaymentsDbContext _dbContext;

    public BrowseWithdrawalAccountsHandler(PaymentsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<PagedResult<WithdrawalAccountDto>> HandleAsync(BrowseWithdrawalAccounts query, CancellationToken cancellationToken = default)
    {
        var accounts = _dbContext.WithdrawalAccounts.AsQueryable();
            
        if (!string.IsNullOrWhiteSpace(query.Currency))
        {
            _ = new Currency(query.Currency);
            accounts = accounts.Where(x => x.Currency == query.Currency);
        }
            
        if (query.CustomerId.HasValue)
        {
            accounts = accounts.Where(x => x.CustomerId == query.CustomerId);
        }

        return accounts.AsNoTracking()
            .OrderByDescending(x => x.CreatedAt)
            .Select(x => new WithdrawalAccountDto
            {
                AccountId = x.Id,
                CustomerId = x.CustomerId,
                Currency = x.Currency,
                Iban = x.Iban,
                CreatedAt = x.CreatedAt
            })
            .PaginateAsync(query, cancellationToken);
    }
}