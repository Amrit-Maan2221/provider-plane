using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TenantRegistry.Domain.Entities;

namespace TenantRegistry.Infrastructure.Persistence.Configurations;

public class TenantContactConfiguration : IEntityTypeConfiguration<TenantContact>
{
    public void Configure(EntityTypeBuilder<TenantContact> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Name).IsRequired().HasMaxLength(200);
        builder.Property(c => c.Email).IsRequired().HasMaxLength(200);
        builder.Property(c => c.Phone).HasMaxLength(50);

        builder.Property<Guid>("TenantId");

        builder.HasIndex("TenantId");

        builder.HasIndex("TenantId")
               .HasFilter("[IsPrimary] = 1")
               .IsUnique();
    }
}
