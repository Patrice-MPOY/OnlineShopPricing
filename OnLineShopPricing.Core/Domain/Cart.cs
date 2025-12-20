using OnlineShopPricing.Core.Services;


namespace OnlineShopPricing.Core.Domain
{
    /// <summary>
    /// Constructs a cart for a specific customer with an injected pricing strategy.
    /// Manual injection is used instead of a full DI container for simplicity in this standalone exercise,
    /// while still demonstrating the Dependency Inversion Principle (DIP).
    /// In a real application (e.g., ASP.NET Core), the strategy would be resolved via the built-in DI container.
    /// </summary>
    public class Cart(Customer client, IPricingStrategy pricingStrategy)
    {
        private readonly Dictionary<ProductType, int> _items = new();
        private readonly Customer _client = client ?? throw new ArgumentNullException(nameof(client));
        private readonly IPricingStrategy _pricingStrategy = pricingStrategy ?? throw new ArgumentNullException(nameof(pricingStrategy));

        public void AddProduct(ProductType product, int quantity)
        {
            if (quantity <= 0) throw new ArgumentException("Quantity must be positive.");

            if (_items.TryGetValue(product, out var current))
                _items[product] = current + quantity;
            else
                _items[product] = quantity;
        }

        public decimal CalculateTotal()
        {
            decimal total = 0m;
            foreach (var item in _items)
            {
                total += _pricingStrategy.GetUnitPrice(item.Key) * item.Value;
            }
            return total;
        }

        public IReadOnlyDictionary<ProductType, int> Items => _items;
    }
}
