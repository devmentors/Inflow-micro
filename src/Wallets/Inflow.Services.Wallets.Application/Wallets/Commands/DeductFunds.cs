using System;
using Convey.CQRS.Commands;

namespace Inflow.Services.Wallets.Application.Wallets.Commands;

public record DeductFunds(Guid WalletId, string Currency, decimal Amount, string TransferName = null,
    string TransferMetadata = null) : ICommand
{
    public Guid TransferId { get; init; } = Guid.NewGuid();
}