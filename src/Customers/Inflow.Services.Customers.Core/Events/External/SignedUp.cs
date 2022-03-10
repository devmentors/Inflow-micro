using System;
using Convey.CQRS.Events;
using Convey.MessageBrokers;

namespace Inflow.Services.Customers.Core.Events.External;

[Message("users")]
internal record SignedUp(Guid UserId, string Email, string Role) : IEvent;