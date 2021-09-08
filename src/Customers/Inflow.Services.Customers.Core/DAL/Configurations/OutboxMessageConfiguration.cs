using System;
using System.Collections.Generic;
using System.Linq;
using Convey.MessageBrokers.Outbox.Messages;
using Inflow.Services.Customers.Core.Infrastructure.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inflow.Services.Customers.Core.DAL.Configurations
{
    internal sealed  class OutboxMessageConfiguration : IEntityTypeConfiguration<OutboxMessage>
    {
        private readonly IJsonSerializer _serializer = new SystemTextJsonSerializer();

        public void Configure(EntityTypeBuilder<OutboxMessage> builder)
        {
            builder.Ignore(x => x.Message);
            builder.Ignore(x => x.MessageContext);

            builder
                .Property(x => x.Headers)
                .HasConversion(x => _serializer.Serialize(x),
                    x => _serializer.Deserialize<Dictionary<string, object>>(x));
            
            builder
                .Property(x => x.Headers).Metadata.SetValueComparer(
                    new ValueComparer<Dictionary<string, object>>(
                        (c1, c2) => c1.SequenceEqual(c2),
                        c => c.Aggregate(0, (a, next) => HashCode.Combine(a, next.GetHashCode()))));
        }
    }
}