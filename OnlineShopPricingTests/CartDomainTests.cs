using FluentAssertions;
using OnlineShopPricing.Core.Domain;
using OnlineShopPricing.Core.Domain.Exceptions;
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
            total.Should().Be(0m, "an empty cart contains no billable items");
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
        public void Cart_WhenCustomerIsNull_ShouldThrowArgumentNullException()
        {
            // Act
            Action act = static () => new Cart(null!);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>()
               .And.ParamName.Should().Be("customer");
        }
    }
}
