using System;
using Convey.CQRS.Events;

namespace Inflow.Services.Payments.Core.Withdrawals.Events
{
    [Contract]
    internal record WithdrawalRejected(Guid WithdrawalId, Guid CustomerId, string Currency, decimal Amount) : IEvent;
}