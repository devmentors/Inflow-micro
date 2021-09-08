using System;
using Inflow.Services.Payments.Core.Deposits.Domain.Entities;
using Inflow.Services.Payments.Shared.ValueObjects;

namespace Inflow.Services.Payments.Core.Deposits.Domain.Factories
{
    internal interface IDepositAccountFactory
    {
        DepositAccount Create(Guid customerId, Nationality nationality, Currency currency);
    }
}