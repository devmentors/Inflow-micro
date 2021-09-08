using Inflow.Services.Payments.Core.Deposits.Domain.Entities;
using Inflow.Services.Payments.Shared.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inflow.Services.Payments.Core.DAL.Configurations
{
    internal class DepositConfiguration : IEntityTypeConfiguration<Deposit>
    {
        public void Configure(EntityTypeBuilder<Deposit> builder)
        {
            builder.Property(x => x.Amount).IsRequired()
                .HasConversion(x => x.Value, x => new Amount(x));
            
            builder.Property(x => x.Currency).IsRequired()
                .HasConversion(x => x.Value, x => new Currency(x));
            
            // For PostgreSQL UseXminAsConcurrencyToken() can be used instead
            builder.Property(x => x.ProcessedAt).IsConcurrencyToken();
        }
    }
}