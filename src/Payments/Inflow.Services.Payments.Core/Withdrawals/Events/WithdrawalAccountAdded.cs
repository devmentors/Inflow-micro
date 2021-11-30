using System;
using Convey.CQRS.Events;

namespace Inflow.Services.Payments.Core.Withdrawals.Events;

[Contract]
internal record WithdrawalAccountAdded(Guid AccountId, Guid CustomerId, string Currency) : IEvent;