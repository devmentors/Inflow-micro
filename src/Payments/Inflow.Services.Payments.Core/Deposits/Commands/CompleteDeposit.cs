using System;
using Convey.CQRS.Commands;

namespace Inflow.Services.Payments.Core.Deposits.Commands
{
    public record CompleteDeposit(Guid DepositId, string Secret) : ICommand;
}