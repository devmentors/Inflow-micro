using System;
using Convey.CQRS.Events;
using Convey.MessageBrokers;

namespace Inflow.Services.Wallets.Application.Owners.Events.External
{
    [Message("customers")]
    internal record CustomerVerified(Guid CustomerId) : IEvent;
}