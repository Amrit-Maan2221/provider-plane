namespace ProductCatalog.Contracts.Events;

public record ProductCreated
(
    Guid ProductId,
    string Code,
    string Name,
    string Category
);
