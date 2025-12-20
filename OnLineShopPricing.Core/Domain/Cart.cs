using OnlineShopPricing.Core.Services;


namespace OnlineShopPricing.Core.Domain
{
    /// <summary>
    /// Represents a shopping cart for a customer. Acts as an aggregate root in the domain.
    /// 
    /// Uses constructor injection for the pricing strategy to demonstrate the Dependency Inversion Principle (DIP).
    /// In this exercise, injection is manual for simplicity; in a real application (e.g., ASP.NET Core),
    /// the strategy would be resolved via the built-in dependency injection container.
    /// </summary>
    public class Cart(Customer customer, IPricingStrategy pricingStrategy)
    {
        private readonly Dictionary<ProductType, int> _items = [];
        
        private readonly IPricingStrategy _pricingStrategy = pricingStrategy ?? throw new ArgumentNullException(nameof(pricingStrategy));

        public void AddProduct(ProductType product, int quantity)
        {
            if (quantity <= 0) throw new ArgumentException("Quantity must be positive.");

            if (_items.TryGetValue(product, out var current))
                _items[product] = current + quantity;
            else
                _items[product] = quantity;
        }        

        public decimal CalculateTotal() =>
            _items.Sum(item => _pricingStrategy.GetUnitPrice(item.Key) * item.Value);

        public IReadOnlyDictionary<ProductType, int> Items => _items;

        public Customer Customer => customer;
    }
}
