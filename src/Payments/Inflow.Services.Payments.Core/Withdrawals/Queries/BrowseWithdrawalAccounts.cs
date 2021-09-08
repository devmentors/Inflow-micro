using System;
using Convey.CQRS.Queries;
using Inflow.Services.Payments.Core.Withdrawals.DTO;

namespace Inflow.Services.Payments.Core.Withdrawals.Queries
{
    public class BrowseWithdrawalAccounts : PagedQueryBase, IQuery<PagedResult<WithdrawalAccountDto>>
    {
        public Guid? CustomerId { get; set; }
        public string Currency { get; set; }
    }
}