using OnlineShopPricing.Core.Domain;

namespace OnlineShopPricing.Core.Services
{
    public class LargeBusinessPricingStrategy : IPricingStrategy
    {
        private static readonly Dictionary<ProductType, decimal> Prices = new()
    {
        { ProductType.HighEndPhone, 1000m },
        { ProductType.MidRangePhone, 550m },
        { ProductType.Laptop, 900m }
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
