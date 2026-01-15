using OnlineShopPricing.Core.Domain;
using OnlineShopPricing.Core.Domain.ValueObjects;

namespace OnlineShopPricing.Core.Services;

/// <summary>
/// Pricing strategy for large business customers (B2B - high volume)
/// Significantly lower unit prices compared to retail/individual customers
/// </summary>
public class LargeBusinessPricingStrategy : PricingStrategyBase
{
    // Static readonly dictionary - initialized once, immutable, thread-safe for reads
    private static readonly IReadOnlyDictionary<ProductType, Money> _prices =
        new Dictionary<ProductType, Money>
            {
                // Business prices are noticeably lower than retail prices
                { ProductType.HighEndPhone,    new Money(1000m) },
                { ProductType.MidRangePhone,   new Money(550m)  },
                { ProductType.Laptop,          new Money(900m)  }
            }
            .AsReadOnly();

    // Required implementation from PricingStrategyBase
    // All concrete strategies must provide their price mapping
    protected override IReadOnlyDictionary<ProductType, Money> Prices => _prices;
}