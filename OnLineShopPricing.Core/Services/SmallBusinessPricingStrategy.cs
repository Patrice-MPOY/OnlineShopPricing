using OnlineShopPricing.Core.Domain;
using OnlineShopPricing.Core.Domain.ValueObjects;

namespace OnlineShopPricing.Core.Services;

/// <summary>
/// Pricing strategy for small/medium business customers (B2B - moderate volume)
/// Prices are between individual retail and large enterprise discounts
/// </summary>
public class SmallBusinessPricingStrategy : PricingStrategyBase
{
    // Static readonly collection - initialized once at class load time
    // Immutable, thread-safe for concurrent reads, and cannot be modified later
    private static readonly IReadOnlyDictionary<ProductType, Money> _prices =
        new Dictionary<ProductType, Money>
            {
                // Small business prices offer a moderate discount compared to retail
                { ProductType.HighEndPhone,    new Money(1150m) },
                { ProductType.MidRangePhone,   new Money(600m)  },
                { ProductType.Laptop,          new Money(1000m) }
            }
            .AsReadOnly();

    // Required override from the abstract base class
    // All pricing strategy implementations must expose their price table this way
    protected override IReadOnlyDictionary<ProductType, Money> Prices => _prices;
}