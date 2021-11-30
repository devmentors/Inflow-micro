using System.Runtime.CompilerServices;
using Convey;

[assembly: InternalsVisibleTo("Inflow.Services.Wallets.Api")]
[assembly: InternalsVisibleTo("Inflow.Services.Wallets.Infrastructure")]
[assembly: InternalsVisibleTo("Inflow.Services.Wallets.Tests.EndToEnd")]
[assembly: InternalsVisibleTo("Inflow.Services.Wallets.Tests.Integration")]
[assembly: InternalsVisibleTo("Inflow.Services.Wallets.Tests.Shared")]
[assembly: InternalsVisibleTo("Inflow.Services.Wallets.Tests.Unit")]
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]

namespace Inflow.Services.Wallets.Application;

internal static class Extensions
{
    public static IConveyBuilder AddApplication(this IConveyBuilder builder)
    {
        return builder;
    }
}