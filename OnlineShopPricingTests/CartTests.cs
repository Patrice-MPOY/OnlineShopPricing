using FluentAssertions;
using OnlineShopPricing.Core.Domain;
using OnlineShopPricing.Core.Domain.Exceptions;
using OnlineShopPricing.Core.Resources;

namespace OnlineShopPricingTests
{
    /// <summary>
    /// Unit and light integration tests for the Cart class.
    /// Since the pricing strategy is now determined polymorphically by the Customer itself
    /// (via GetPricingStrategy()), we no longer mock IPricingStrategy.
    /// All tests use real customers and real pricing strategies to validate actual domain behavior.
    /// </summary>
    public class CartTests
    {
        // =============================================================================
        // Core behavior tests – using real domain logic (polymorphic pricing strategy)
        // =============================================================================

        [Fact]
        public void EmptyCart_ReturnsZero()
        {
            // Arrange
            // A freshly created cart with no items should always return zero,
            // regardless of the customer type or pricing strategy.
            var customer = new IndividualCustomer("TEST001", "Test", "User");
            var cart = new Cart(customer);

            // Act & Assert
            cart.CalculateTotal().Should().Be(0m, "an empty cart has no items to price");
        }

        [Fact]
        public void AddProduct_MultipleTimes_AccumulatesQuantityCorrectly()
        {
            // Arrange
            // This test verifies that calling AddProduct several times for the same product
            // correctly accumulates the quantity in the internal dictionary.
            var customer = new IndividualCustomer("TEST001", "Test", "User");
            var cart = new Cart(customer);

            // Act
            cart.AddProduct(ProductType.MidRangePhone, 2);
            cart.AddProduct(ProductType.MidRangePhone, 3);

            // Assert
            // MidRangePhone price for IndividualCustomer = 800m (from IndividualPricingStrategy)
            // Total = 5 units × 800m = 4000m
            cart.CalculateTotal().Should().Be(4000m);
        }

        [Fact]
        public void AddProduct_WithUnknownProduct_ThrowsInvalidProductTypeException()
        {
            // Arrange
            // The pricing strategy only knows the defined ProductTypes.
            // Adding a product not present in the strategy should be rejected early.
            var customer = new IndividualCustomer("TEST001", "Test", "User");
            var cart = new Cart(customer);

            // Act
            // We simulate an invalid/corrupted ProductType by casting an arbitrary int.
            Action act = () => cart.AddProduct((ProductType)999, 1);

            // Assert
            // The guard in Cart.AddProduct uses TryGetUnitPrice to validate the product.
            act.Should().ThrowExactly<InvalidProductTypeException>();
        }

        [Fact]
        public void CalculateTotal_WithVeryLargeQuantity_HandlesLargeNumbersCorrectly()
        {
            // Arrange
            // This test ensures no overflow or precision loss occurs with large quantities.
            // decimal type is used for money, so multiplication is safe.
            var customer = new IndividualCustomer("TEST001", "Test", "User");
            var cart = new Cart(customer);

            const int hugeQuantity = int.MaxValue / 2; // Safe value to avoid theoretical issues
            cart.AddProduct(ProductType.HighEndPhone, hugeQuantity);

            // Act
            decimal total = cart.CalculateTotal();

            // Assert
            // HighEndPhone price for Individual = 1500m
            decimal expected = hugeQuantity * 1500m;
            total.Should().Be(expected, "decimal multiplication should handle large quantities accurately");
        }

        [Fact]
        public void AddProduct_WhenQuantityIsZeroOrNegative_ThrowsInvalidQuantityException()
        {
            // Arrange
            var customer = new IndividualCustomer("TEST001", "Test", "User");
            var cart = new Cart(customer);

            // Act & Assert - Negative quantity
            Action actNegative = () => cart.AddProduct(ProductType.Laptop, -5);
            actNegative.Should().ThrowExactly<InvalidQuantityException>()
                       .WithMessage(ErrorMessages.QuantityMustBePositive + "*");

            // Act & Assert - Zero quantity
            Action actZero = () => cart.AddProduct(ProductType.Laptop, 0);
            actZero.Should().ThrowExactly<InvalidQuantityException>()
                   .WithMessage(ErrorMessages.QuantityMustBePositive + "*");
        }
    }
}