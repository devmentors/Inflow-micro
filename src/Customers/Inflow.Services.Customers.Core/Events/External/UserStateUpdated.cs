using System;
using Convey.CQRS.Events;
using Convey.MessageBrokers;

namespace Inflow.Services.Customers.Core.Events.External;

[Message("users")]
internal record UserStateUpdated(Guid UserId, string State) : IEvent;