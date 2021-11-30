using System;
using Convey.CQRS.Events;
using Convey.MessageBrokers;

namespace Inflow.Saga.Api.Messages;

[Message("customers")]
internal record CustomerVerified(Guid CustomerId) : IEvent;