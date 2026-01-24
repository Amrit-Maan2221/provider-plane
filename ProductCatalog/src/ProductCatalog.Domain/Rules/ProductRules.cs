using ProductCatalog.Domain.Enums;

namespace ProductCatalog.Domain.Rules;

public static class ProductRules
{
    public static void EnsureCanActivate(ProductStatus status)
    {
        if (status == ProductStatus.Retired)
            throw new InvalidOperationException(
                "A retired product cannot be activated."
            );
    }

    public static void EnsureCanAddPlan(ProductStatus status)
    {
        if (status != ProductStatus.Active)
            throw new InvalidOperationException(
                "Plans can only be added to active products."
            );
    }
}
