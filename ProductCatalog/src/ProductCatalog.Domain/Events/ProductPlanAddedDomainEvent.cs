namespace ProductCatalog.Domain.Events;

public class ProductPlanAddedDomainEvent : DomainEventBase
{
    public Guid ProductId { get; }
    public Guid PlanId { get; }

    public ProductPlanAddedDomainEvent(Guid productId, Guid planId)
    {
        ProductId = productId;
        PlanId = planId;
    }
}
