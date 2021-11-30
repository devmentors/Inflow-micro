using Convey.CQRS.Queries;
using Inflow.Services.Wallets.Application.Wallets.DTO;

namespace Inflow.Services.Wallets.Application.Wallets.Queries;

public class BrowseTransfers : PagedQueryBase, IQuery<PagedResult<TransferDto>>
{
    public string Currency { get; set; }
    public string Name { get; set; }
}