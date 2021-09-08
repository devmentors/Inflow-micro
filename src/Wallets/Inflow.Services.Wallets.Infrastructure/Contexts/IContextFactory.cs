using Inflow.Services.Wallets.Application.Contexts;

namespace Inflow.Services.Wallets.Infrastructure.Contexts
{
    internal interface IContextFactory
    {
        IContext Create();
    }
}