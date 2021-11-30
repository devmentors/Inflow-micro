using System;
using Inflow.Services.Users.Core.DTO;

namespace Inflow.Services.Users.Core.Services;

public interface IUserRequestStorage
{
    void SetToken(Guid commandId, JwtDto jwt);
    JwtDto GetToken(Guid commandId);
}