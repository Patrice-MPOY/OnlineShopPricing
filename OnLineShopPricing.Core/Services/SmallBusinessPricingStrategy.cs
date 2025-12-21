using OnlineShopPricing.Core.Domain;

namespace OnlineShopPricing.Core.Services
{
    public class SmallBusinessPricingStrategy : PricingStrategyBase
    {
        protected override IReadOnlyDictionary<ProductType, decimal> Prices =>
            new Dictionary<ProductType, decimal>
            {
            { ProductType.HighEndPhone, 1150m },
            { ProductType.MidRangePhone, 600m },
            { ProductType.Laptop, 1000m }
            };       
    }
}
