using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Inflow.APIGateway.Serialization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace Inflow.APIGateway.Identity;

internal sealed class UserMiddleware : IMiddleware
{
    private readonly IJsonSerializer _jsonSerializer;

    private static readonly ISet<string> ValidMethods = new HashSet<string>
    {
        "POST", "PUT", "PATCH"
    };

    public UserMiddleware(IJsonSerializer jsonSerializer)
    {
        _jsonSerializer = jsonSerializer;
    }
        
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var request = context.Request;
        if (!ValidMethods.Contains(request.Method))
        {
            await next(context);
            return;
        }

        if (!request.Headers.ContainsKey("authorization"))
        {
            await next(context);
            return;
        }

        var path = context.Request.Path.Value;
        if (path is not null && (path.Contains("account/sign-in") || path.Contains("account/sign-up")))
        {
            await next(context);
            return;
        }

        var authenticateResult = await context.AuthenticateAsync();
        if (!authenticateResult.Succeeded || authenticateResult.Principal is null)
        {
            context.Response.StatusCode = 401;
            return;
        }

        string content;
        context.User = authenticateResult.Principal;
        using (var reader = new StreamReader(request.Body))
        {
            content = await reader.ReadToEndAsync();
        }

        var payload = _jsonSerializer.Deserialize<Dictionary<string, object>>(content);
        if (payload is null || context.User.Identity is null || string.IsNullOrWhiteSpace(context.User.Identity.Name))
        {
            await next(context);
            return;
        }

        var userId = context.User.Identity?.Name;
        if (string.IsNullOrWhiteSpace(userId))
        {
            await next(context);
            return;
        }

        payload["userId"] = userId;
        var json = _jsonSerializer.Serialize(payload);
        await using var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(json));
        context.Request.Body = memoryStream;
        context.Request.ContentLength = json.Length;
        await next(context);
    }
}