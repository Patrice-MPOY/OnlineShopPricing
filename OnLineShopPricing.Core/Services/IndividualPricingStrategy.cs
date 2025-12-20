using OnlineShopPricing.Core.Domain;

namespace OnlineShopPricing.Core.Services
{

    public class IndividualPricingStrategy : IPricingStrategy
    {
        private static readonly Dictionary<ProductType, decimal> Prices = new()
    {
        { ProductType.HighEndPhone, 1500m },
        { ProductType.MidRangePhone, 800m },
        { ProductType.Laptop, 1200m }
    };

        public decimal GetUnitPrice(ProductType product)
        {
            return Prices.TryGetValue(product, out var price)
                ? price
                : throw new ArgumentException($"Unknown product: {product}");
        }
    }
}
