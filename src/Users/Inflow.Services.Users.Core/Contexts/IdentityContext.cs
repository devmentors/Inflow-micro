using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Inflow.Services.Users.Core.Contexts;

internal sealed class IdentityContext : IIdentityContext
{
    public bool IsAuthenticated { get; }
    public Guid Id { get; }
    public string Role { get; }
    public Dictionary<string, IEnumerable<string>> Claims { get; }

    private IdentityContext()
    {
    }
        
    public IdentityContext(CorrelationContext.UserContext context)
        : this(context.Id, context.Role, context.IsAuthenticated, context.Claims)
    {
    }
        
    public IdentityContext(string id, string role, bool isAuthenticated, Dictionary<string, IEnumerable<string>> claims)
    {
        Id = Guid.TryParse(id, out var userId) ? userId : Guid.Empty;
        Role = role ?? string.Empty;
        IsAuthenticated = isAuthenticated;
        Claims = claims ?? new Dictionary<string, IEnumerable<string>>();
    }

    public IdentityContext(Guid? id)
    {
        Id = id ?? Guid.Empty;
        IsAuthenticated = id.HasValue;
    }

    public IdentityContext(ClaimsPrincipal principal)
    {
        if (principal?.Identity is null || string.IsNullOrWhiteSpace(principal.Identity.Name))
        {
            return;
        }
            
        IsAuthenticated = principal.Identity?.IsAuthenticated is true;
        Id = IsAuthenticated ? Guid.Parse(principal.Identity.Name) : Guid.Empty;
        Role = principal.Claims.SingleOrDefault(x => x.Type == ClaimTypes.Role)?.Value;
        Claims = principal.Claims.GroupBy(x => x.Type)
            .ToDictionary(x => x.Key, x => x.Select(c => c.Value.ToString()));
    }
        
    public bool IsUser() => Role is "user";
        
    public bool IsAdmin() => Role is "admin";
        
    public static IIdentityContext Empty => new IdentityContext();
}