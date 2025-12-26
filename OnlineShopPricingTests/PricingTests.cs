using FluentAssertions;
using OnlineShopPricing.Core.Domain;
using OnlineShopPricing.Core.Domain.Exceptions;
using OnlineShopPricing.Core.Resources;
using OnlineShopPricing.Core.Services;
using OnlineShopPricing.Tests.TestHelpers;

namespace OnlineShopPricingTests
{
    /// <summary>
    /// These tests validate pricing behavior using real pricing strategy implementations.
    /// They are written to remain readable and deterministic while covering the core
    /// pricing rules of the system.
    ///
    /// The Cart supports dependency injection of IPricingStrategy, which allows
    /// mocking in larger applications to achieve more focused unit tests.
    /// </summary>
    public class PricingTests
    {
        // Shared test data for parameterized pricing tests
        public static IEnumerable<object[]> CalculateTotalTestData => new List<object[]>
        {
            // Individual customer pricing
            new object[]
            {
                new IndividualCustomer("IND001", "John", "Doe"),
                new (ProductType Product, int Quantity)[]
                {
                    (ProductType.HighEndPhone, 1),
                    (ProductType.Laptop, 2)
                },
                3900m
            },

            // Small business pricing (annual turnover < 10M)
            new object[]
            {
                new BusinessCustomer("BIZ001", "Small Corp", "REG123", 5_000_000m),
                new (ProductType Product, int Quantity)[]
                {
                    (ProductType.HighEndPhone, 1),
                    (ProductType.MidRangePhone, 3),
                    (ProductType.Laptop, 1)
                },
                3950m
            },

            // Large business pricing (annual turnover ≥ 10M)
            new object[]
            {
                new BusinessCustomer("BIZ002", "Large Corp", "REG456", 15_000_000m),
                new (ProductType Product, int Quantity)[]
                {
                    (ProductType.HighEndPhone, 2),
                    (ProductType.MidRangePhone, 1),
                    (ProductType.Laptop, 5)
                },
                7050m
            }
        };

        [Theory]
        [MemberData(nameof(CalculateTotalTestData))]
        public void CalculateTotal_WhenCustomerTypeChanges_ShouldApplyCorrectPricing(
            Customer customer,
            (ProductType Product, int Quantity)[] itemsToAdd,
            decimal expectedTotal)
        {
            // Arrange
            var pricingStrategy = PricingStrategyFactory.CreateStrategy(customer);
            var cart = new Cart(customer, pricingStrategy);

            // Act
            foreach (var (product, quantity) in itemsToAdd)
            {
                cart.AddProduct(product, quantity);
            }

            var total = cart.CalculateTotal();

            // Assert
            total.Should().Be(expectedTotal);
        }

        [Fact]
        public void AddProduct_WhenQuantityIsNegative_ShouldThrowInvalidQuantityException()
        {
            // Arrange
            var customer = new IndividualCustomer("IND001", "John", "Doe");
            var pricingStrategy = PricingStrategyFactory.CreateStrategy(customer);
            var cart = new Cart(customer, pricingStrategy);

            // Act
            Action act = () => cart.AddProduct(ProductType.Laptop, -1);

            // Assert
            act.Should().Throw<InvalidQuantityException>()
                .WithMessage(ErrorMessages.QuantityMustBePositive + "*");
            //.And.ParamName.Should().Be("quantity");
        }

        [Fact]
        public void CreateStrategy_WhenCustomerIsNull_ShouldThrowArgumentNullException()
        {
            // Act
            Action act = () => PricingStrategyFactory.CreateStrategy(null!);

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void CreateStrategy_WhenCustomerTypeIsInvalid_ShouldThrowArgumentException()
        {
            // Arrange
            var invalidCustomer = new InvalidCustomerType();

            // Act
            Action act = () => PricingStrategyFactory.CreateStrategy(invalidCustomer);

            // Assert
            act.Should().Throw<ArgumentException>()
               .WithMessage(ErrorMessages.InvalidCustomerType + "*")
               .And.ParamName.Should().Be("customer");
        }
    }
}
