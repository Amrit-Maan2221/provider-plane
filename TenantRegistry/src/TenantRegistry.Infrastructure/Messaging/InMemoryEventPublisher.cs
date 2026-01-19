using TenantRegistry.Application.Abstractions.Messaging;

namespace TenantRegistry.Infrastructure.Messaging;

public class InMemoryEventPublisher : IEventPublisher
{
    public Task PublishAsync<TEvent>(TEvent integrationEvent, CancellationToken ct = default)
        where TEvent : class
    {
        Console.WriteLine($"[EVENT PUBLISHED] {typeof(TEvent).Name}");
        return Task.CompletedTask;
    }
}
