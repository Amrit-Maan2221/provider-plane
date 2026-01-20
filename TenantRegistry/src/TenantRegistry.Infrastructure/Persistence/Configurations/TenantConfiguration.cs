using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TenantRegistry.Domain.Entities;

namespace TenantRegistry.Infrastructure.Persistence.Configurations;

public class TenantConfiguration : IEntityTypeConfiguration<Tenant>
{
    public void Configure(EntityTypeBuilder<Tenant> builder)
    {
        builder.ToTable("Tenants");

        builder.HasKey(t => t.TenantId);

        builder.Property(t => t.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(t => t.Slug)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasIndex(t => t.Slug)
            .IsUnique();

        builder.Property(t => t.Status)
            .IsRequired();

        builder.Property(t => t.Country)
            .HasMaxLength(50);

        builder.Property(t => t.Timezone)
            .HasMaxLength(50);

        builder.Property(t => t.CreatedAt)
            .IsRequired();

        builder.Property(t => t.UpdatedAt)
            .IsRequired();

        builder
            .Navigation(t => t.Contacts)
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder
            .HasMany(t => t.Contacts)
            .WithOne()
            .HasForeignKey("TenantId")
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .Navigation(t => t.Settings)
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder
            .HasMany(t => t.Settings)
            .WithOne()
            .HasForeignKey("TenantId")
            .OnDelete(DeleteBehavior.Cascade);
    }
}
