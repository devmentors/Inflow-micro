using System;
using Convey.CQRS.Events;
using Convey.MessageBrokers;

namespace Inflow.Services.Wallets.Application.Wallets.Events.External;

[Message("payments")]
internal record DepositAccountAdded(Guid AccountId, Guid CustomerId, string Currency) : IEvent;