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
