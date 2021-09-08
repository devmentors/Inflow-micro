using System;
using Convey.CQRS.Events;

namespace Inflow.Services.Payments.Core.Deposits.Events
{
    [Contract]
    internal record DepositCompleted(Guid DepositId, Guid CustomerId, string Currency, decimal Amount) : IEvent;
}