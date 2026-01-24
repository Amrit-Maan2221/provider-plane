using ProductCatalog.Domain.Enums;
using ProductCatalog.Domain.Events;
using ProductCatalog.Domain.Rules;

namespace ProductCatalog.Domain.Entities;

public class Product : Entity
{
    public Guid ProductId { get; private set; }
    public string Code { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public string Category { get; private set; }
    public ProductStatus Status { get; private set; }

    private readonly List<ProductPlan> _plans = new();
    public IReadOnlyCollection<ProductPlan> Plans => _plans.AsReadOnly();

    private readonly List<ProductFeature> _features = new();
    public IReadOnlyCollection<ProductFeature> Features => _features.AsReadOnly();

    protected Product() { }

    public Product(string code, string name, string description, string category)
    {
        ProductId = Guid.NewGuid();
        Code = code;
        Name = name;
        Description = description;
        Category = category;
        Status = ProductStatus.Draft;

        AddDomainEvent(new ProductCreatedDomainEvent(ProductId));
    }

    public void Activate()
    {
        ProductRules.EnsureCanActivate(Status);

        Status = ProductStatus.Active;
        AddDomainEvent(new ProductActivatedDomainEvent(ProductId));
    }

    public void Retire()
    {
        Status = ProductStatus.Retired;
        AddDomainEvent(new ProductRetiredDomainEvent(ProductId));
    }

    public void AddPlan(ProductPlan plan)
    {
        ProductRules.EnsureCanAddPlan(Status);

        _plans.Add(plan);
        AddDomainEvent(new ProductPlanAddedDomainEvent(ProductId, plan.PlanId));
    }

    public void AddFeature(ProductFeature feature)
    {
        _features.Add(feature);
    }
}
