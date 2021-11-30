using System;
using Convey.CQRS.Events;
using Convey.MessageBrokers;

namespace Inflow.Saga.Api.Messages;

[Message("payments")]
internal record DepositCompleted(Guid DepositId, Guid CustomerId, string Currency, decimal Amount) : IEvent;