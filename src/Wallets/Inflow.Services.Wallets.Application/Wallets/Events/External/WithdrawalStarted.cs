using System;
using Convey.CQRS.Events;
using Convey.MessageBrokers;

namespace Inflow.Services.Wallets.Application.Wallets.Events.External;

[Message("payments")]
internal record WithdrawalStarted(Guid WithdrawalId, Guid CustomerId, string Currency, decimal Amount) : IEvent;