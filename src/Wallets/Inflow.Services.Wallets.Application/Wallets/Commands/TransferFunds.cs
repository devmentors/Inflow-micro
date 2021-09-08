using System;
using Convey.CQRS.Commands;

namespace Inflow.Services.Wallets.Application.Wallets.Commands
{
    public record TransferFunds(Guid OwnerId, Guid OwnerWalletId, Guid ReceiverWalletId, string Currency,
        decimal Amount) : ICommand
    {
        public Guid OutgoingTransferId { get; init; } = Guid.NewGuid();
        public Guid IncomingTransferId { get; init; } = Guid.NewGuid();
    }
}