namespace ProductCatalog.Domain.Events;

public class ProductCreatedDomainEvent : DomainEventBase
{
    public Guid ProductId { get; }

    public ProductCreatedDomainEvent(Guid productId)
    {
        ProductId = productId;
    }
}
