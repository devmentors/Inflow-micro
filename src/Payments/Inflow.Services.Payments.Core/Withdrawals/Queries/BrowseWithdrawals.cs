using System;
using Convey.CQRS.Queries;
using Inflow.Services.Payments.Core.Withdrawals.DTO;

namespace Inflow.Services.Payments.Core.Withdrawals.Queries;

public class BrowseWithdrawals : PagedQueryBase, IQuery<PagedResult<WithdrawalDto>>
{
    public Guid? AccountId { get; set; }
    public Guid? CustomerId { get; set; }
    public string Currency { get; set; }
    public string Status { get; set; }
}