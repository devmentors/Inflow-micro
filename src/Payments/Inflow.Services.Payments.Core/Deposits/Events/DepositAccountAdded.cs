using System;
using Convey.CQRS.Events;

namespace Inflow.Services.Payments.Core.Deposits.Events
{
    [Contract]
    internal record DepositAccountAdded(Guid AccountId, Guid CustomerId, string Currency) : IEvent;
}