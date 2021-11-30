using System;

namespace Inflow.Services.Payments.Core.Withdrawals.DTO;

public class WithdrawalAccountDto
{
    public Guid AccountId { get; set; }
    public Guid CustomerId { get; set; }
    public string Currency { get; set; }
    public string Iban { get; set; }
    public DateTime CreatedAt { get; set; }
}