using System;
using System.ComponentModel.DataAnnotations;
using Convey.CQRS.Commands;

namespace Inflow.Services.Users.Core.Commands
{
    public record SignIn([Required] [EmailAddress] string Email, [Required] string Password) : ICommand
    {
        public Guid Id { get; init; } = Guid.NewGuid();
    }
}