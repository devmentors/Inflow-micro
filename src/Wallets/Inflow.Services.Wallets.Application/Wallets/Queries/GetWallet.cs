using System;
using Convey.CQRS.Queries;
using Inflow.Services.Wallets.Application.Wallets.DTO;

namespace Inflow.Services.Wallets.Application.Wallets.Queries;

public class GetWallet : IQuery<WalletDetailsDto>
{
    public Guid WalletId { get; set; }
}