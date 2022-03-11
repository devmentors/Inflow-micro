using System;
using Convey.CQRS.Events;
using Convey.MessageBrokers;

namespace Inflow.Saga.Api.Messages;

[Message("users")]
internal record SignedUp(Guid UserId, string Email, string Role) : IEvent;