using System;
using Convey.CQRS.Events;

namespace Inflow.Services.Users.Core.Events;

[Contract]
internal record SignedUp(Guid UserId, string Email, string Role) : IEvent;