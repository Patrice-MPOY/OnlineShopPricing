using FluentAssertions;
using Moq;
using OnlineShopPricing.Core.Domain;
using OnlineShopPricing.Core.Resources;
using OnlineShopPricing.Core.Services;

namespace OnlineShopPricing.Tests;
public class CartTests
{
    // ==================================================================================
    // Isolated Unit Tests (using mocked IPricingStrategy)
    // Purpose: Test the pure mechanics of the Cart independently of actual pricing rules
    // ===================================================================================
    [Fact]
    public void CalculateTotal_WithMockedStrategy_UsesProvidedPrices()
    {
        // Arrange
        // Create a mock of IPricingStrategy to isolate the Cart from real pricing logic.
        // This allows us to test the Cart's behavior (quantity accumulation and total calculation)
        // independently of the actual pricing rules.
        var mockStrategy = new Mock<IPricingStrategy>();

        // Setup TryGetUnitPrice to return true for any product.
        // This is necessary because Cart.AddProduct validates the product existence using TryGetUnitPrice.
        // Without this setup, AddProduct would throw an exception for "unknown product".
        mockStrategy.Setup(s => s.TryGetUnitPrice(It.IsAny<ProductType>(), out It.Ref<decimal>.IsAny))
                    .Returns(true);

        // Setup specific unit prices for the products used in this test.
        // These values are arbitrary but consistent with the expected total.
        mockStrategy.Setup(s => s.GetUnitPrice(ProductType.HighEndPhone)).Returns(1000m);
        mockStrategy.Setup(s => s.GetUnitPrice(ProductType.Laptop)).Returns(500m);

        // Create a test customer – any valid customer instance is fine here
        // since the test focuses on the cart mechanics, not customer-specific rules.
        var client = new IndividualCustomer("TEST001", "Test", "User");

        // Instantiate the Cart with the mocked strategy to achieve full isolation.
        var cart = new Cart(client, mockStrategy.Object);

        // Act
        // Add products to the cart.
        // The first call adds 1 HighEndPhone, the second adds 2 Laptops (quantity accumulation).
        cart.AddProduct(ProductType.HighEndPhone, 1);
        cart.AddProduct(ProductType.Laptop, 2);

        // Assert
        // Verify that the total is correctly calculated based on the mocked prices:
        // 1 * 1000 + 2 * 500 = 2000
        cart.CalculateTotal().Should().Be(2000m);

        // Verify that GetUnitPrice was called exactly once per distinct product type.
        // This confirms that the pricing strategy is used correctly and efficiently
        // (one call per product type, not per unit).
        mockStrategy.Verify(s => s.GetUnitPrice(ProductType.HighEndPhone), Times.Once);
        mockStrategy.Verify(s => s.GetUnitPrice(ProductType.Laptop), Times.Once);

        // Ensure no unexpected calls were made to the strategy.
        // This protects against future regressions.
        
    }

    [Fact]
    public void EmptyCart_ReturnsZero()
    {
        // Arrange
        var mockStrategy = new Mock<IPricingStrategy>();
        var client = new IndividualCustomer("TEST", "Test", "User");
        var cart = new Cart(client, mockStrategy.Object);

        // Act & Assert
        cart.CalculateTotal().Should().Be(0m);
    }

    
    [Fact]
    public void AddProduct_MultipleTimes_AccumulatesQuantity()
    {
        // Arrange
        var mockStrategy = new Mock<IPricingStrategy>();
        // Setup pour autoriser le produit utilisé dans le test
        mockStrategy.Setup(s => s.TryGetUnitPrice(ProductType.MidRangePhone, out It.Ref<decimal>.IsAny))
                    .Returns((ProductType p, out decimal price) => { price = 100m; return true; });
        mockStrategy.Setup(s => s.GetUnitPrice(ProductType.MidRangePhone)).Returns(100m);

        var client = new IndividualCustomer("TEST", "Test", "User");
        var cart = new Cart(client, mockStrategy.Object);

        // Act
        cart.AddProduct(ProductType.MidRangePhone, 2);
        cart.AddProduct(ProductType.MidRangePhone, 3);

        // Assert
        cart.CalculateTotal().Should().Be(500m); // 5 * 100
    }


    // =====================================================================================
    // Light Integration Tests (using real pricing strategy)
    // Purpose: Validate real-world interaction between Cart and a concrete pricing strategy
    // =====================================================================================

    [Fact]
    
    public void Add_UnknownProduct_ThrowsArgumentException()
    {
        // Arrange
        // Create a real pricing strategy and a cart.
        // The strategy knows only the three valid products.
        var strategy = new IndividualPricingStrategy();
        var customer = new IndividualCustomer("TEST", "Test", "User");
        var cart = new Cart(customer, strategy);

        // Act
        // Attempt to add a product with an invalid ProductType (forced cast to simulate corruption or bug)
        Action act = () => cart.AddProduct((ProductType)999, 1);

        // Assert
        // Verify that the Cart rejects the unknown product with the centralized error message
        // from resources. Using the resource ensures the test remains in sync if the message changes
        // (e.g., for internationalization or UX improvements).
        act.Should().Throw<ArgumentException>()
           .WithMessage(string.Format(ErrorMessages.InvalidProductType, "999") + "*")
           .And.ParamName.Should().Be("product");
    }

    [Fact]
    public void CalculateTotal_WithVeryLargeQuantity_ReturnsCorrectTotal()
    {
        // Arrange
        var customer = new IndividualCustomer("ID", "John", "Doe");
        var cart = new Cart(customer, new IndividualPricingStrategy());

        const int hugeQuantity = int.MaxValue / 2; // Safe value to avoid any theoretical overflow risk
        cart.AddProduct(ProductType.HighEndPhone, hugeQuantity);

        // Act
        decimal total = cart.CalculateTotal();

        // Assert (with Fluent Assertions)
        decimal expected = hugeQuantity * 1500m;
        total.Should().Be(expected,
            because: ErrorMessages.VeryLargeQuantityTestExplanation) ;
    }
}