namespace ProductCatalog.Contracts.Events;

public record ProductPlanCreated
(
    Guid ProductId,
    Guid PlanId,
    string PlanCode,
    string BillingCycle,
    decimal? Price,
    string Currency
);
