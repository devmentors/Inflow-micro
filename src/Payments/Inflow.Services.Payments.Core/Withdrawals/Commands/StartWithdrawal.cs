using System;
using Convey.CQRS.Commands;

namespace Inflow.Services.Payments.Core.Withdrawals.Commands;

[Contract]
public record StartWithdrawal(Guid AccountId, Guid CustomerId, string Currency, decimal Amount) : ICommand
{
    public Guid WithdrawalId { get; init; } = Guid.NewGuid();
}