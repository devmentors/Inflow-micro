using System;

namespace Inflow.Services.Payments.Core.Deposits.DTO
{
    public class DepositAccountDto
    {
        public Guid AccountId { get; set; }
        public Guid CustomerId { get; set; }
        public string Currency { get; set; }
        public string Iban { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}