using OnlineShopPricing.Core.Domain;
using OnlineShopPricing.Core.Domain.ValueObjects;
namespace OnlineShopPricing.Core.Services
{
    public interface IPricingStrategy
    {
        /// <summary>
        /// Retourne le prix unitaire d'un type de produit sous forme de Money
        /// </summary>
        Money GetUnitPrice(ProductType product);

        /// <summary>
        /// Version Try pour éviter les exceptions quand on veut juste tester l'existence
        /// </summary>
        bool TryGetUnitPrice(ProductType product, out Money unitPrice);
    }

}
