namespace ProductCatalog.Contracts.Events;

public record ProductActivated
(
    Guid ProductId,
    string Code,
    string Name
);
