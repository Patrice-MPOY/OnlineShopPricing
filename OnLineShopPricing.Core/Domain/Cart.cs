using OnlineShopPricing.Core.Domain.Exceptions;
using OnlineShopPricing.Core.Services;

namespace OnlineShopPricing.Core.Domain
{
    /// <summary>
    /// Represents a shopping cart for a customer. Acts as an aggregate root in the domain.
    /// 
    /// The pricing strategy is resolved polymorphically by the Customer itself 
    /// through the GetPricingStrategy() method (Tell, Don't Ask principle).
    /// This design eliminates the need for an external factory or manual injection,
    /// making the domain more cohesive, extensible, and aligned with SOLID principles.
    /// 
    /// In a real application (e.g., ASP.NET Core), the Cart would be instantiated 
    /// via the DI container with a scoped or transient lifestyle, receiving a fully 
    /// configured Customer instance.
    /// </summary>
    public class Cart(Customer customer)
    {
        private readonly Dictionary<ProductType, int> _items = [];
        private readonly IPricingStrategy _pricingStrategy =
            (customer ?? throw new ArgumentNullException(nameof(customer)))
            .GetPricingStrategy()
            ;
        public IReadOnlyDictionary<ProductType, int> Items => _items;
        public Customer Customer => customer;

        public void AddProduct(ProductType product, int quantity)
        {
            GuardAgainstNonPositiveQuantity(quantity);
            GuardAgainstUnpricedProduct(product);

            IncreaseProductQuantity(product, quantity);
        }
        public decimal CalculateTotal() =>
            _items.Sum(item => _pricingStrategy.GetUnitPrice(item.Key) * item.Value);
        
        private static void GuardAgainstNonPositiveQuantity(int quantity)
        {
            if (quantity <= 0)
            {
                throw new InvalidQuantityException(quantity);
            }
        }
        private void GuardAgainstUnpricedProduct(ProductType product)
        {
            // Validation of ProductType: the product must exist in the current pricing grid
            if (!_pricingStrategy.TryGetUnitPrice(product, out _))
            {
                throw new InvalidProductTypeException(nameof(product));
            }
        }
        private void IncreaseProductQuantity(ProductType product, int quantity)
        {
            _items[product] = _items.GetValueOrDefault(product) + quantity;
        }
    }
}