using ProductCatalog.Domain.Enums;

namespace ProductCatalog.Domain.Entities;

public class ProductPlan
{
    public Guid PlanId { get; private set; }
    public Guid ProductId { get; private set; }
    public string Code { get; private set; }
    public string Name { get; private set; }
    public BillingCycle BillingCycle { get; private set; }
    public decimal? Price { get; private set; }
    public string Currency { get; private set; }
    public PlanStatus Status { get; private set; }
    public int SortOrder { get; private set; }

    private readonly List<PlanFeature> _features = new();
    public IReadOnlyCollection<PlanFeature> Features => _features.AsReadOnly();

    protected ProductPlan() { }

    public ProductPlan(
        Guid productId,
        string code,
        string name,
        BillingCycle billingCycle,
        decimal? price,
        string currency,
        int sortOrder)
    {
        PlanId = Guid.NewGuid();
        ProductId = productId;
        Code = code;
        Name = name;
        BillingCycle = billingCycle;
        Price = price;
        Currency = currency;
        SortOrder = sortOrder;
        Status = PlanStatus.Active;
    }

    public void Deprecate()
    {
        Status = PlanStatus.Deprecated;
    }

    public void AddFeature(PlanFeature feature)
    {
        _features.Add(feature);
    }
}
