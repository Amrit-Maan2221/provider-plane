namespace ProductCatalog.Domain.Entities;

public class PlanFeature
{
    public Guid PlanFeatureId { get; private set; }
    public Guid PlanId { get; private set; }
    public Guid FeatureId { get; private set; }
    public string Value { get; private set; }

    protected PlanFeature() { }

    public PlanFeature(Guid planId, Guid featureId, string value)
    {
        PlanFeatureId = Guid.NewGuid();
        PlanId = planId;
        FeatureId = featureId;
        Value = value;
    }
}
