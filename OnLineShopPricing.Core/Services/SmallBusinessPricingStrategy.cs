using OnlineShopPricing.Core.Domain;

namespace OnlineShopPricing.Core.Services
{

    public class SmallBusinessPricingStrategy : IPricingStrategy
    {
        private static readonly Dictionary<ProductType, decimal> Prices = new()
    {
        { ProductType.HighEndPhone, 1150m },
        { ProductType.MidRangePhone, 600m },
        { ProductType.Laptop, 1000m }
    };

        public bool TryGetUnitPrice(ProductType product, out decimal price)
        {
            return Prices.TryGetValue(product, out price);
        }

        public decimal GetUnitPrice(ProductType product)
        {
            return Prices[product]; // Plus besoin de TryGetValue + exception – validé en amont
        }
    }
}
