using System;
using Convey.CQRS.Events;
using Convey.MessageBrokers;

namespace Inflow.Services.Payments.Core.Deposits.Events.External;

[Message("customers")]
internal record CustomerLocked(Guid CustomerId) : IEvent;