using System;
using Convey.CQRS.Events;

namespace Inflow.Services.Payments.Core.Withdrawals.Events
{
    [Contract]
    internal record WithdrawalStarted(Guid WithdrawalId, Guid CustomerId, string Currency, decimal Amount) : IEvent;
}