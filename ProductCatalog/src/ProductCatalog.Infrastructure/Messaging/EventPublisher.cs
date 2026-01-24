using ProductCatalog.Domain.Events;

namespace ProductCatalog.Infrastructure.Messaging;

public class EventPublisher : IEventPublisher
{
    public Task PublishAsync(IDomainEvent domainEvent, CancellationToken ct)
    {
        Console.WriteLine($"Event published: {domainEvent.GetType().Name} at {domainEvent.OccurredOn}");
        return Task.CompletedTask;
    }
}
