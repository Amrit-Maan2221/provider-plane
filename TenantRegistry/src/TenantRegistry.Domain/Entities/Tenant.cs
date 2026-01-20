using TenantRegistry.Domain.Enums;
using TenantRegistry.Domain.Exceptions;

namespace TenantRegistry.Domain.Entities;

public class Tenant
{
    public Guid TenantId { get; private set; }
    public string Name { get; private set; } = default!;
    public string Slug { get; private set; } = default!;
    public TenantStatus Status { get; private set; }
    public string Country { get; private set; } = default!;
    public string Timezone { get; private set; } = default!;

    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    private readonly List<TenantContact> _contacts = new();

    private readonly List<TenantSetting> _settings = new();

    public IReadOnlyCollection<TenantContact> Contacts => _contacts;

    public IReadOnlyCollection<TenantSetting> Settings => _settings.AsReadOnly();

    private Tenant() { } // Required for EF Core

    // ---------- Factory ----------
    public static Tenant Create(
        string name,
        string slug,
        string country,
        string timezone)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Tenant name is required");

        if (string.IsNullOrWhiteSpace(slug))
            throw new DomainException("Tenant slug is required");

        return new Tenant
        {
            TenantId = Guid.NewGuid(),
            Name = name.Trim(),
            Slug = slug.Trim().ToLowerInvariant(),
            Country = country,
            Timezone = timezone,
            Status = TenantStatus.Pending,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }

    // ---------- Lifecycle ----------
    public void Activate()
    {
        if (Status != TenantStatus.Pending)
            throw new DomainException("Only pending tenants can be activated");

        Status = TenantStatus.Active;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Suspend()
    {
        if (Status != TenantStatus.Active)
            throw new DomainException("Only active tenants can be suspended");

        Status = TenantStatus.Suspended;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Reactivate()
    {
        if (Status != TenantStatus.Suspended)
            throw new DomainException("Only suspended tenants can be reactivated");

        Status = TenantStatus.Active;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Close()
    {
        if (Status != TenantStatus.Active)
            throw new DomainException("Only active tenants can be closed");

        Status = TenantStatus.Closed;
        UpdatedAt = DateTime.UtcNow;
    }

    // ---------- Profile Updates ----------
    public void UpdateProfile(
        string name,
        string country,
        string timezone)
    {
        Name = name;
        Country = country;
        Timezone = timezone;
        UpdatedAt = DateTime.UtcNow;
    }


    public void AddContacts(IEnumerable<TenantContact> contacts)
    {
        if (!contacts.Any(c => c.IsPrimary))
            throw new DomainException("Primary contact is required.");

        if (contacts.Count(c => c.IsPrimary) > 1)
            throw new DomainException("Only one primary contact allowed.");

        _contacts.AddRange(contacts);
    }

    public void ReplaceSettings(IEnumerable<TenantSetting> settings)
    {
        _settings.Clear();
        _settings.AddRange(settings);
    }

    // Add or update single setting
    public void SetSetting(string key, string value)
    {
        var existing = _settings.FirstOrDefault(x => x.Key == key);

        if (existing is null)
        {
            _settings.Add(new TenantSetting(key, value));
        }
        else
        {
            existing.UpdateValue(value);
        }
    }

    public void RemoveSetting(string key)
    {
        var existing = _settings.FirstOrDefault(x => x.Key == key);
        if (existing != null)
        {
            _settings.Remove(existing);
        }
    }
}
