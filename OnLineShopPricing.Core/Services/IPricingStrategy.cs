using OnlineShopPricing.Core.Domain;

namespace OnlineShopPricing.Core.Services
{
    public interface IPricingStrategy
    {
        decimal GetUnitPrice(ProductType product);
    }
}
