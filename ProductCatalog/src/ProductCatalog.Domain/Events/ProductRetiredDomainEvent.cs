namespace ProductCatalog.Domain.Events;

public class ProductRetiredDomainEvent : DomainEventBase
{
    public Guid ProductId { get; }

    public ProductRetiredDomainEvent(Guid productId)
    {
        ProductId = productId;
    }
}
