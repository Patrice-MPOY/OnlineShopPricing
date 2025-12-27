using OnlineShopPricing.Core.Domain;

namespace OnlineShopPricing.Core.Services
{
    public class IndividualPricingStrategy : PricingStrategyBase
    {
        private static readonly IReadOnlyDictionary<ProductType, decimal> _prices =
            new Dictionary<ProductType, decimal>
            {
            { ProductType.HighEndPhone, 1500m },
            { ProductType.MidRangePhone, 800m },
            { ProductType.Laptop, 1200m }
            };
        protected override IReadOnlyDictionary<ProductType, decimal> Prices => _prices;
    }
}
