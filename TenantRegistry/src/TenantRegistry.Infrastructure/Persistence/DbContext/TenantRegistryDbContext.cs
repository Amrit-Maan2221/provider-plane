using TenantRegistry.Domain.Entities;

namespace TenantRegistry.Infrastructure.Persistence.DbContext;

public class TenantRegistryDbContext : Microsoft.EntityFrameworkCore.DbContext
{
    public TenantRegistryDbContext(Microsoft.EntityFrameworkCore.DbContextOptions<TenantRegistryDbContext> options) : base(options)
    {  
    }

    public Microsoft.EntityFrameworkCore.DbSet<Tenant> Tenants => Set<Tenant>();
    public Microsoft.EntityFrameworkCore.DbSet<TenantContact> TenantContact => Set<TenantContact>();

    protected override void OnModelCreating(Microsoft.EntityFrameworkCore.ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(TenantRegistryDbContext).Assembly);
    }
}