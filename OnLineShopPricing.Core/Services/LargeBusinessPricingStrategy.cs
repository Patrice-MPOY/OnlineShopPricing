using OnlineShopPricing.Core.Domain;

namespace OnlineShopPricing.Core.Services
{
    public class LargeBusinessPricingStrategy : PricingStrategyBase
    {
        protected override IReadOnlyDictionary<ProductType, decimal> Prices =>
            new Dictionary<ProductType, decimal>
            {
            { ProductType.HighEndPhone, 1000m },
            { ProductType.MidRangePhone, 550m },
            { ProductType.Laptop, 900m }
            };
    }
}
