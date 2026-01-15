using OnlineShopPricing.Core.Domain;
using OnlineShopPricing.Core.Domain.ValueObjects;

namespace OnlineShopPricing.Core.Services;

/// <summary>
/// Pricing strategy for individual (retail/B2C) customers
/// Fixed prices, relatively high (no volume discounts)
/// </summary>
public class IndividualPricingStrategy : PricingStrategyBase
{
    // Static readonly dictionary for thread-safety and performance
    // Initialized only once when the class is loaded
    // Using AsReadOnly() to prevent any accidental modification
    private static readonly IReadOnlyDictionary<ProductType, Money> _prices =
        new Dictionary<ProductType, Money>
            {
                // All prices are wrapped in Money right from initialization
                // → domain invariants (no negative amounts) are protected from the start
                { ProductType.HighEndPhone,    new Money(1500m) },
                { ProductType.MidRangePhone,   new Money(800m)  },
                { ProductType.Laptop,          new Money(1200m) }
            }
            .AsReadOnly();

    // Required implementation from the base class
    // All pricing strategies must expose their price table this way
    protected override IReadOnlyDictionary<ProductType, Money> Prices => _prices;
}