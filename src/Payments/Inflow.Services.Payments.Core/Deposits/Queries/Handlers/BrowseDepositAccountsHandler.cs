using System.Linq;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using Inflow.Services.Payments.Core.DAL;
using Inflow.Services.Payments.Core.Deposits.DTO;
using Inflow.Services.Payments.Shared.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Inflow.Services.Payments.Core.Deposits.Queries.Handlers
{
    internal sealed class BrowseDepositAccountsHandler : IQueryHandler<BrowseDepositAccounts, PagedResult<DepositAccountDto>>
    {
        private readonly PaymentsDbContext _dbContext;

        public BrowseDepositAccountsHandler(PaymentsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<PagedResult<DepositAccountDto>> HandleAsync(BrowseDepositAccounts query)
        {
            var accounts = _dbContext.DepositAccounts.AsQueryable();
            
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
                .Select(x => new DepositAccountDto
                {
                    AccountId = x.Id,
                    CustomerId = x.CustomerId,
                    Currency = x.Currency,
                    Iban = x.Iban,
                    CreatedAt = x.CreatedAt
                })
                .PaginateAsync(query);
        }
    }
}