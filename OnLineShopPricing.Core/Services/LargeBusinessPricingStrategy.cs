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

        public decimal GetUnitPrice(ProductType product)
        {
            return Prices.TryGetValue(product, out var price)
                ? price
                : throw new ArgumentException($"Unknown product: {product}");
        }
    }
}
