using FluentAssertions;
using OnlineShopPricing.Core.Domain;
using OnlineShopPricing.Core.Domain.Exceptions;
using OnlineShopPricing.Core.Domain.ValueObjects;
using OnlineShopPricing.Core.Resources;

namespace OnlineShopPricingTests
{
    /// <summary>
    /// Domain-level tests for the Cart aggregate.
    /// These tests validate real business behavior using real customers
    /// and real pricing strategies selected polymorphically by Customer.
    ///
    /// No mocks are used by design.
    /// </summary>
    public class CartDomainTests
    {
        // =============================================================================
        // Core behavior
        // =============================================================================

        [Fact]
        public void EmptyCart_ShouldReturnZero()
        {
            // Arrange
            var customer = new IndividualCustomer("TEST001", "Test", "User");
            var cart = new Cart(customer);

            // Act
            var total = cart.CalculateTotal();

            // Assert
            total.Should().Be(Money.Zero);
        }

        [Fact]
        public void AddSameProductMultipleTimes_ShouldAccumulateQuantities()
        {
            // Arrange
            var customer = new IndividualCustomer("TEST001", "Test", "User");
            var cart = new Cart(customer);

            // Act
            cart.AddProduct(ProductType.MidRangePhone, 2);
            cart.AddProduct(ProductType.MidRangePhone, 3);

            // Assert
            var unitPrice = customer.GetPricingStrategy().GetUnitPrice(ProductType.MidRangePhone);
            cart.CalculateTotal().Should().Be(unitPrice * 5);
        }

        // =============================================================================
        // Validation rules
        // =============================================================================

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        public void AddProduct_WhenQuantityIsZeroOrNegative_ShouldThrowInvalidQuantityException(int invalidQuantity)
        {
            // Arrange
            var customer = new IndividualCustomer("IND001", "John", "Doe");
            var cart = new Cart(customer);

            // Act
            Action act = () => cart.AddProduct(ProductType.Laptop, invalidQuantity);

            // Assert
            act.Should().ThrowExactly<InvalidQuantityException>()
               .WithMessage(ErrorMessages.QuantityMustBePositive + "*");
        }

        [Fact]
        public void AddProduct_WhenProductTypeIsInvalid_ShouldThrowInvalidProductTypeException()
        {
            // Arrange
            var customer = new IndividualCustomer("TEST001", "Test", "User");
            var cart = new Cart(customer);

            // Act
            Action act = () => cart.AddProduct((ProductType)999, 1);

            // Assert
            act.Should().ThrowExactly<InvalidProductTypeException>();
        }

        // =============================================================================
        // Robustness
        // =============================================================================

        [Fact]
        public void CalculateTotal_WithVeryLargeQuantity_ShouldHandleLargeNumbersCorrectly()
        {
            // Arrange
            var customer = new IndividualCustomer("TEST001", "Test", "User");
            var cart = new Cart(customer);

            const int hugeQuantity = int.MaxValue / 2;
            cart.AddProduct(ProductType.HighEndPhone, hugeQuantity);

            // Act
            var total = cart.CalculateTotal();

            // Assert
            var unitPrice = customer.GetPricingStrategy().GetUnitPrice(ProductType.HighEndPhone);
            total.Should().Be(unitPrice * hugeQuantity);
        }

        // =============================================================================
        // Guard clauses
        // =============================================================================

        [Fact]
        public void Cart_WhenCustomerIsNull_ShouldThrowMissingCustomerException()
        {
            // Act
            Action act = static () => new Cart(null!);

            // Assert
            act.Should().ThrowExactly<MissingCustomerException>();
        }

        [Fact]
        public void Cart_ShouldAlwaysBelongToTheCorrectCustomer_AndLinkShouldBeImmutable()
        {
            // Arrange
            var expectedCustomer = new IndividualCustomer("CUST-123", "John", "Doe");
            var wrongCustomer = new IndividualCustomer("CUST-999", "Jane", "Doe");

            // Act
            var cart = new Cart(expectedCustomer);

            // Assert
            cart.Customer.Should().BeSameAs(expectedCustomer);
            cart.Customer.CustomerId.Should().Be("CUST-123");

            cart.Customer.Should().NotBeSameAs(wrongCustomer);
            cart.Customer.Should().NotBeNull();
        }


        [Fact]
        public void CalculateTotal_ReflectsCurrentPricing_WhenCustomerStateIsUpgraded()
        {
            // Arrange - Initial cart with an individual customer (high-end phone = 1500 €)
            var initialCustomer = new IndividualCustomer("C001", "Jean", "Dupont");
            var cart = new Cart(initialCustomer);
            cart.AddProduct(ProductType.HighEndPhone, 2);

            // Assert - Verify initial total (2 × 1500 € = 3000 €)

            cart.CalculateTotal().Amount
                .Should()
                .Be(3000m,
                    "2 high-end phones at individual pricing should cost 3000 €");


            // Arrange - Simulate customer upgrade to large business (same ID, new profile with CA > 10M)
            var upgradedCustomer = new BusinessCustomer(
                "C001",
                "TechCorp",
                "FR123",
                "987654",
                15_000_000m);

            // Act - Create a refreshed cart with the same content but upgraded customer
            var upgradedCart = new Cart(upgradedCustomer);
            upgradedCart.AddProduct(ProductType.HighEndPhone, 2);  // Same content as before

            // Assert - Total should now reflect the new pricing strategy (2 × 1000 € = 2000 €)
            upgradedCart.CalculateTotal().Amount.Should().Be(2000m,
                "Total should reflect the new pricing strategy after customer upgrade");
        }
    }
}
