using Inflow.Services.Wallets.Core.Owners.Entities;
using Inflow.Services.Wallets.Core.Owners.Types;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inflow.Services.Wallets.Infrastructure.EF.Configurations;

internal class IndividualOwnerConfiguration : IEntityTypeConfiguration<IndividualOwner>
{
    public void Configure(EntityTypeBuilder<IndividualOwner> builder)
    {
        builder.Property(x => x.FullName)
            .IsRequired()
            .HasConversion(x => x.Value, x => new FullName(x));
    }
}