using OnlineShopPricing.Core.Domain.Events;
using OnlineShopPricing.Core.Domain.Exceptions;
using OnlineShopPricing.Core.Domain.SeedWork;
using OnlineShopPricing.Core.Domain.ValueObjects;
using OnlineShopPricing.Core.Services;

namespace OnlineShopPricing.Core.Domain
{
    /// <summary>
    /// Represents a shopping cart for a customer.
    /// Acts as an aggregate root in the domain.
    ///
    /// Invariants:
    /// - A cart always belongs to a Customer.
    /// - A cart cannot contain products with non-positive quantities.
    /// - A cart can only contain products priced by the active pricing strategy.
    /// </summary>
    
    public class Cart(Customer customer) : AggregateRoot<Guid>(Guid.NewGuid())
    {
        /// <summary>
        /// The customer owning this cart. Read-only and guaranteed non-null.
        /// </summary>
        public Customer Customer { get; } =
            customer ?? throw new MissingCustomerException("A cart cannot be created without a valid customer.");

        private readonly Dictionary<ProductType, int> _productQuantities = new();

        /// <summary>
        /// Exposes the cart content in a read-only way.
        /// Prevents external mutation while allowing inspection.
        /// </summary>
        public IReadOnlyDictionary<ProductType, int> Items =>
            _productQuantities.AsReadOnly();


        /// <summary>
        /// Adds a positive quantity of a priced product to the cart.
        /// Enforces domain rules before mutation.
        /// </summary>
        public void AddProduct(ProductType product, int quantity)
        {
            GuardAgainstNonPositiveQuantity(quantity);

            var pricingStrategy = GetCurrentPricingStrategy();
            GuardAgainstUnpricedProduct(product, pricingStrategy);

            IncreaseProductQuantity(product, quantity);
        }

        /// <summary>
        /// Calculates the current total using the customer's current pricing strategy.
        /// Prices are dynamically resolved on every call (non-frozen / dynamic approach).
        /// </summary>
        public Money CalculateTotal()
        {
            var pricingStrategy = GetCurrentPricingStrategy();
            var total = Money.Zero;

            foreach (var (product, quantity) in _productQuantities)
            {
                var unitPrice = pricingStrategy.GetUnitPrice(product);
                total += unitPrice * quantity;
            }

            return total;
        }

        // -----------------------
        // Invariant enforcement
        // -----------------------

        private static void GuardAgainstNonPositiveQuantity(int quantity)
        {
            if (quantity <= 0)
            {
                throw new InvalidQuantityException(quantity);
            }
        }
        /// <summary>
        /// Guards against adding a product that is not priced by the current strategy.
        /// </summary>
        /// <param name="product">The product type to validate.</param>
        /// <param name="pricingStrategy">The strategy to check against.</param>
        /// <exception cref="InvalidProductTypeException">If the product has no price in the strategy.</exception>
        private static void GuardAgainstUnpricedProduct(ProductType product, IPricingStrategy pricingStrategy)
        {
            if (!pricingStrategy.TryGetUnitPrice(product, out _))
            {
                throw new InvalidProductTypeException(
                    $"Product '{product}' is not priced by the current strategy.");
            }
        }
        /// <summary>
        /// Returns the pricing strategy currently applicable to this customer.
        /// Resolved dynamically every time to reflect possible changes in customer state.
        /// </summary>
        /// <returns>The active IPricingStrategy.</returns>
        /// <exception cref="InvalidOperationException">If the strategy is null (should never happen if domain is consistent).</exception>
        private IPricingStrategy GetCurrentPricingStrategy()
        {
            return Customer.GetPricingStrategy()
                   ?? throw new MissingPricingStrategyException();
        }

        /// <summary>
        /// Centralizes quantity mutation and enforces invariants defensively.
        /// </summary>
        private void IncreaseProductQuantity(ProductType product, int quantity)
        {
            var currentQuantity = _productQuantities.GetValueOrDefault(product);
            var newQuantity = currentQuantity + quantity;

            if (newQuantity <= 0)
            {
                throw new InvalidQuantityException(
                    newQuantity);
            }

            _productQuantities[product] = newQuantity;

            // Émission de l'événement ici → juste après la mutation réussie
            AddDomainEvent(new ProductAddedToCart(
                CartId: Id,
                CustomerId: Customer.CustomerId,                   
                Product: product,
                QuantityAdded: quantity,
                NewTotalQuantity: newQuantity,
                UnitPrice: GetCurrentPricingStrategy().GetUnitPrice(product),
                OccurredOn: DateTime.UtcNow
            ));
        }
    }
}
