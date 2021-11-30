using System;
using Convey.CQRS.Commands;

namespace Inflow.Services.Payments.Core.Withdrawals.Commands;

public record CompleteWithdrawal(Guid WithdrawalId, string Secret) : ICommand;