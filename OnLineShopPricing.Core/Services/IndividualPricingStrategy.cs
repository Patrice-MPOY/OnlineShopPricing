using OnlineShopPricing.Core.Domain;

namespace OnlineShopPricing.Core.Services
{
    public class IndividualPricingStrategy : PricingStrategyBase
    {
        protected override IReadOnlyDictionary<ProductType, decimal> Prices =>
            new Dictionary<ProductType, decimal>
            {
            { ProductType.HighEndPhone, 1500m },
            { ProductType.MidRangePhone, 800m },
            { ProductType.Laptop, 1200m }
            };
    }
}
