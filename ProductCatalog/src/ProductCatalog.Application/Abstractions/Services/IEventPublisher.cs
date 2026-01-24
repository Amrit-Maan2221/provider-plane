using ProductCatalog.Domain.Events;

public interface IEventPublisher
{
    Task PublishAsync(IDomainEvent domainEvent, CancellationToken ct);
}