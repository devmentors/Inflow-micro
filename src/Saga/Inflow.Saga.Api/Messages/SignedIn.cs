using System;
using Convey.CQRS.Events;
using Convey.MessageBrokers;

namespace Inflow.Saga.Api.Messages;

[Message("users")]
internal record SignedIn(Guid UserId) : IEvent;