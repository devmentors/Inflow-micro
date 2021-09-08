using System;
using Convey.CQRS.Events;
using Convey.MessageBrokers;

namespace Inflow.Saga.Api.Messages
{
    [Message("wallets")]
    internal record WalletAdded(Guid WalletId, Guid OwnerId, string Currency) : IEvent;
}