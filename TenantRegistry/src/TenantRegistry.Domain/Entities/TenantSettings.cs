namespace TenantRegistry.Domain.Entities;

public class TenantSetting
{
    public string Key { get; private set; } = default!;
    public string Value { get; private set; } = default!;

    protected TenantSetting() { } // EF

    public TenantSetting(string key, string value)
    {
        if (string.IsNullOrWhiteSpace(key))
            throw new ArgumentException("Setting key cannot be empty");

        Key = key;
        Value = value;
    }

    public void UpdateValue(string value)
    {
        Value = value;
    }
}
