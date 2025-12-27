using OnlineShopPricing.Core.Domain;
namespace OnlineShopPricing.Core.Services
{
    public class LargeBusinessPricingStrategy : PricingStrategyBase
    {
        private static readonly IReadOnlyDictionary<ProductType, decimal> _prices =
            new Dictionary<ProductType, decimal>
            {
            { ProductType.HighEndPhone, 1000m },
            { ProductType.MidRangePhone, 550m },
            { ProductType.Laptop, 900m }
            };
        protected override IReadOnlyDictionary<ProductType, decimal> Prices => _prices;
    }
}
