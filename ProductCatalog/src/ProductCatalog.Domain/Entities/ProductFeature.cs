using ProductCatalog.Domain.Enums;

namespace ProductCatalog.Domain.Entities;

public class ProductFeature
{
    public Guid FeatureId { get; private set; }
    public Guid ProductId { get; private set; }
    public string Key { get; private set; }
    public string Description { get; private set; }
    public FeatureType Type { get; private set; }
    public bool IsMetered { get; private set; }

    protected ProductFeature() { }

    public ProductFeature(
        Guid productId,
        string key,
        string description,
        FeatureType type,
        bool isMetered)
    {
        FeatureId = Guid.NewGuid();
        ProductId = productId;
        Key = key;
        Description = description;
        Type = type;
        IsMetered = isMetered;
    }
}
