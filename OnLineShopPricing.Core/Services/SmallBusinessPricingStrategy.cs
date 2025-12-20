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

        public decimal GetUnitPrice(ProductType product)
        {
            return Prices.TryGetValue(product, out var price)
                ? price
                : throw new ArgumentException($"Unknown product: {product}");
        }
    }
}
