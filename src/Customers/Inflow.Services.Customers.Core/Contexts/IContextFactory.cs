namespace Inflow.Services.Customers.Core.Contexts;

internal interface IContextFactory
{
    IContext Create();
}