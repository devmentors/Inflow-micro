using System;
using Convey.CQRS.Events;
using Convey.MessageBrokers;

namespace Inflow.Saga.Api.Messages;

[Message("wallets")]
internal record FundsAdded(Guid WalletId, Guid OwnerId, string Currency, decimal Amount, string TransferName = null,
    string TransferMetadata = null) : IEvent;