using System;
using Convey.CQRS.Queries;
using Inflow.Services.Wallets.Application.Wallets.DTO;

namespace Inflow.Services.Wallets.Application.Wallets.Queries;

public class BrowseWallets : PagedQueryBase, IQuery<PagedResult<WalletDto>>
{
    public Guid? OwnerId { get; set; }
    public string Currency { get; set; }
}