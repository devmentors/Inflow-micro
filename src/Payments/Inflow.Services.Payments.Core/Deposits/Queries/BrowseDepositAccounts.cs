using System;
using Convey.CQRS.Queries;
using Inflow.Services.Payments.Core.Deposits.DTO;

namespace Inflow.Services.Payments.Core.Deposits.Queries
{
    public class BrowseDepositAccounts : PagedQueryBase, IQuery<PagedResult<DepositAccountDto>>
    {
        public Guid? CustomerId { get; set; }
        public string Currency { get; set; }
    }
}