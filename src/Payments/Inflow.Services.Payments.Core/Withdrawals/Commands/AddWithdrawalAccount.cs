using System;
using Convey.CQRS.Commands;

namespace Inflow.Services.Payments.Core.Withdrawals.Commands;

public record AddWithdrawalAccount(Guid CustomerId, string Currency, string Iban) : ICommand
{
    public Guid AccountId { get; init; } = Guid.NewGuid();
}