using System;
using Convey.CQRS.Events;
using Convey.MessageBrokers;

namespace Inflow.Saga.Api.Messages
{
    [Message("customers")]
    internal record CustomerCompleted(Guid CustomerId, string Name, string FullName, string Nationality) : IEvent;
}