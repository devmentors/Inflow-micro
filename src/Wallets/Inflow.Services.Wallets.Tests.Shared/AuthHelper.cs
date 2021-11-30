using System;
using System.Collections.Generic;
using Convey.Auth;
using Inflow.Services.Wallets.Infrastructure.Time;

namespace Inflow.Services.Wallets.Tests.Shared;

public static class AuthHelper
{
    private static readonly AuthManager AuthManager;

    static AuthHelper()
    {
        var options = OptionsHelper.GetOptions<JwtOptions>("jwt");
        AuthManager = new AuthManager(options, new UtcClock());
    }

    public static string GenerateJwt(Guid userId, string role = null, string audience = null,
        IDictionary<string, IEnumerable<string>> claims = null)
        => AuthManager.CreateToken(userId, role, audience, claims).AccessToken;
}