namespace Inflow.Services.Payments.Core.Contexts;

internal interface IContextFactory
{
    IContext Create();
}