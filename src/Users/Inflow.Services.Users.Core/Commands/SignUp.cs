using System;
using System.ComponentModel.DataAnnotations;
using Convey.CQRS.Commands;

namespace Inflow.Services.Users.Core.Commands;

public record SignUp([Required] [EmailAddress] string Email, [Required] string Password, string Role) : ICommand
{
    public Guid UserId { get; init; } = Guid.NewGuid();
}