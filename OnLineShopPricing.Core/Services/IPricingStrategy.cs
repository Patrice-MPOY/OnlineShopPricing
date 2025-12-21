using OnlineShopPricing.Core.Domain;

namespace OnlineShopPricing.Core.Services
{   

    public interface IPricingStrategy
    {
        /// <summary>
        /// Gets the unit price for the product. Throws if the product is unknown.
        /// </summary>
        decimal GetUnitPrice(ProductType product);

        /// <summary>
        /// Tries to get the unit price without throwing. Used for validation without side effects.
        /// </summary>
        bool TryGetUnitPrice(ProductType product, out decimal price);
    }
}
