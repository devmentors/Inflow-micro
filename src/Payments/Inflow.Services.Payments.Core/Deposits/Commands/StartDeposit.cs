using System;
using Convey.CQRS.Commands;

namespace Inflow.Services.Payments.Core.Deposits.Commands
{
    [Contract]
    public record StartDeposit(Guid AccountId, Guid CustomerId, string Currency, decimal Amount) : ICommand
    {
        public Guid DepositId { get; init; } = Guid.NewGuid();
    }
}