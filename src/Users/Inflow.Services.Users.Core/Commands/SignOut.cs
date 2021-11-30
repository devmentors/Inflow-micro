using System;
using Convey.CQRS.Commands;

namespace Inflow.Services.Users.Core.Commands;

public record SignOut(Guid UserId) : ICommand;