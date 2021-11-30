using System;
using Inflow.Services.Payments.Core.Deposits.Domain.Entities;
using Inflow.Services.Payments.Core.Services;
using Inflow.Services.Payments.Shared.ValueObjects;

namespace Inflow.Services.Payments.Core.Deposits.Domain.Factories;

internal class DepositAccountFactory : IDepositAccountFactory
{
    private static readonly Random Random = new();
    private readonly IClock _clock;

    public DepositAccountFactory(IClock clock)
    {
        _clock = clock;
    }

    public DepositAccount Create(Guid customerId, Nationality nationality, Currency currency)
    {
        var iban = $"{nationality.Value}0000{Random.Next(int.MaxValue)}0000{Random.Next(int.MaxValue)}0000";

        return new DepositAccount(Guid.NewGuid(), customerId, currency, iban, _clock.CurrentDate());
    }
}