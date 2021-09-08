using System;
using Convey.CQRS.Events;

namespace Inflow.Services.Users.Core.Events
{
    [Contract]
    internal record SignedIn(Guid UserId) : IEvent;
}