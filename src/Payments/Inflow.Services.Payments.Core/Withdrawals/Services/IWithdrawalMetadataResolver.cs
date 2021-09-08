using System;

namespace Inflow.Services.Payments.Core.Withdrawals.Services
{
    internal interface IWithdrawalMetadataResolver
    {
        Guid? TryResolveWithdrawalId(string metadata);
    }
}