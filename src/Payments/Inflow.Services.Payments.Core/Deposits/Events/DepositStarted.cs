using System;
using Convey.CQRS.Events;

namespace Inflow.Services.Payments.Core.Deposits.Events
{
    [Contract]
    internal record DepositStarted(Guid DepositId, Guid CustomerId, string Currency, decimal Amount) : IEvent;
}