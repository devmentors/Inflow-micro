using Inflow.Services.Wallets.Core.Wallets.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inflow.Services.Wallets.Infrastructure.EF.Configurations
{
    internal class OutgoingTransferConfiguration : IEntityTypeConfiguration<OutgoingTransfer>
    {
        public void Configure(EntityTypeBuilder<OutgoingTransfer> builder)
        {
        }
    }
}