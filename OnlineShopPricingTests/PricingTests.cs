using FluentAssertions;
using OnlineShopPricing.Core.Domain;
using OnlineShopPricing.Core.Resources;
using OnlineShopPricing.Core.Services;
using OnlineShopPricing.Tests.TestHelpers;

namespace OnlineShopPricingTests
{
    /// <summary>
    /// These tests use real pricing strategy implementations to validate end-to-end behavior
    /// and keep the test suite simple and readable for this limited-scope exercise.     
    /// The design fully supports mocking IPricingStrategy (via constructor injection),
    /// which would be used in a larger application to isolate the Cart and achieve faster, more focused unit tests.
    /// </summary>
    public class PricingTests
    {
        private readonly PricingStrategyFactory _factory = new();
        [Fact]
        public void Cart_WithIndividualCustomer_AppliesCorrectPricing()
        {
            // Arrange
            var client = new IndividualCustomer("IND001", "John", "Doe");
            var strategy = _factory.CreateStrategy(client);
            var cart = new Cart(client, strategy);

            // Act
            cart.AddProduct(ProductType.HighEndPhone, 1);
            cart.AddProduct(ProductType.Laptop, 2);

            // Assert
            cart.CalculateTotal().Should().Be(3900m);
        }

        [Fact]
        public void Cart_WithSmallBusinessCustomer_AppliesCorrectPricing()
        {
            // Arrange
            var client = new BusinessCustomer("BIZ001", "Small Corp", "REG123", 5_000_000m);
            var strategy = _factory.CreateStrategy(client);
            var cart = new Cart(client, strategy);

            // Act
            cart.AddProduct(ProductType.HighEndPhone, 1);
            cart.AddProduct(ProductType.MidRangePhone, 3);
            cart.AddProduct(ProductType.Laptop, 1);

            // Assert
            cart.CalculateTotal().Should().Be(3950m); // 1150 + 3*600 + 1000 = 1150 + 1800 + 1000 = 3950
        }

        [Fact]
        public void Cart_WithLargeBusinessCustomer_AppliesCorrectPricing()
        {
            // Arrange
            var client = new BusinessCustomer("BIZ002", "Large Corp", "REG456", 15_000_000m);
            var strategy = _factory.CreateStrategy(client);
            var cart = new Cart(client, strategy);

            // Act
            cart.AddProduct(ProductType.HighEndPhone, 2);
            cart.AddProduct(ProductType.MidRangePhone, 1);
            cart.AddProduct(ProductType.Laptop, 5);

            // Assert
            cart.CalculateTotal().Should().Be(7050m);
            // Correct calculation: 2*1000 (HighEndPhone) + 1*550 (MidRangePhone) + 5*900 (Laptop) = 2000 + 550 + 4500 = 7050
        }
        

        [Fact]
        public void AddProduct_WithNegativeQuantity_ThrowsException()
        {
            // Arrange
            var client = new IndividualCustomer("IND001", "John", "Doe");
            var strategy = _factory.CreateStrategy(client);
            var cart = new Cart(client, strategy);

            // Act & Assert
            Action act = () => cart.AddProduct(ProductType.Laptop, -1);

            act.Should().Throw<ArgumentException>()
               .WithMessage(ErrorMessages.QuantityMustBePositive + "*") // Utilise la ressource + wildcard
               .And.ParamName.Should().Be("quantity");
        }


        [Fact]
        public void CreateStrategy_WithNullClient_ThrowsArgumentNullException()
        {
            // Act & Assert
            Action act = () => _factory.CreateStrategy(null!);
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void CreateStrategy_WithUnsupportedClientType_ThrowsArgumentException()
        {
            // Arrange - A fictiv class
            var unsupportedClient = new UnsupportedCustomer();

            // Act & Assert
            Action act = () => _factory.CreateStrategy(unsupportedClient);
            act.Should().Throw<ArgumentException>()
               .WithMessage("Unsupported client type*");
        }

        //[Fact]
        //public void PricingStrategy_WithUnknownProduct_ThrowsArgumentException()
        //{
        //    // Arrange
        //    var strategy = new IndividualPricingStrategy();

        //    // Act & Assert
        //    Action act = () => strategy.GetUnitPrice((ProductType)999); // unknown product

        //    act.Should().Throw<ArgumentException>()
        //       .WithMessage("Unknown product: 999*");
        //}
    }
}
