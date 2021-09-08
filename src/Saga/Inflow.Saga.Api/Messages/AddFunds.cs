using System;
using Convey.CQRS.Commands;
using Convey.MessageBrokers;

namespace Inflow.Saga.Api.Messages
{
    [Message("wallets")]
    internal record AddFunds(Guid WalletId, string Currency, decimal Amount, string TransferName = null,
        string TransferMetadata = null) : ICommand
    {
        public Guid TransferId { get; init; } = Guid.NewGuid();
    }
}