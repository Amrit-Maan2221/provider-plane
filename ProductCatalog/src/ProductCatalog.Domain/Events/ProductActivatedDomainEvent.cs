namespace ProductCatalog.Domain.Events;

public class ProductActivatedDomainEvent : DomainEventBase
{
    public Guid ProductId { get; }

    public ProductActivatedDomainEvent(Guid productId)
    {
        ProductId = productId;
    }
}
