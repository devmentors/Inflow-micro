using System;
using Convey.CQRS.Events;
using Convey.MessageBrokers;

namespace Inflow.Services.Payments.Core.Withdrawals.Events.External
{
    [Message("wallets")]
    internal record FundsDeducted(Guid WalletId, Guid OwnerId, string Currency, decimal Amount, string TransferName = null,
        string TransferMetadata = null) : IEvent;
}