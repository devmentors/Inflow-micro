using System;
using Convey.CQRS.Queries;
using Inflow.Services.Payments.Core.Deposits.DTO;

namespace Inflow.Services.Payments.Core.Deposits.Queries;

public class BrowseDeposits : PagedQueryBase, IQuery<PagedResult<DepositDto>>
{
    public Guid? AccountId { get; set; }
    public Guid? CustomerId { get; set; }
    public string Currency { get; set; }
    public string Status { get; set; }
}