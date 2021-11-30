using System.Collections.Generic;

namespace Inflow.Services.Users.Core.DTO;

public class UserDetailsDto : UserDto
{
    public IEnumerable<string> Permissions { get; set; }
}