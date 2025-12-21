using OnlineShopPricing.Core.Domain;


namespace OnlineShopPricing.Core.Services
{   

    public abstract class PricingStrategyBase : IPricingStrategy
    {
        protected abstract IReadOnlyDictionary<ProductType, decimal> Prices { get; }

        public bool TryGetUnitPrice(ProductType product, out decimal price)
        {
            return Prices.TryGetValue(product, out price);
        }

        public decimal GetUnitPrice(ProductType product)
        {
            return Prices[product];
        }
    }

}
