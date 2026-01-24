namespace ProductCatalog.Contracts.Dtos;

public record ProductDto
(
    Guid ProductId,
    string Code,
    string Name,
    string Description,
    string Category,
    string Status
);
