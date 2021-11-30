using System;

namespace Inflow.Services.Customers.Core.Clients.DTO;

internal class UserDto
{
    public Guid UserId { get; set; }
    public string Email { get; set; }
    public string Role { get; set; }
}