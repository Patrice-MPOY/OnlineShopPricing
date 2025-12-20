using FluentAssertions;
using Moq;
using OnlineShopPricing.Core.Domain;
using OnlineShopPricing.Core.Services;

namespace OnlineShopPricing.Tests;
public class CartTests
{    

    [Fact]
    public void CalculateTotal_UsesInjectedStrategy_IsolatedWithMock()
    {
        // Arrange
        var mockStrategy = new Mock<IPricingStrategy>();
        mockStrategy.Setup(s => s.GetUnitPrice(ProductType.HighEndPhone)).Returns(1000m);
        mockStrategy.Setup(s => s.GetUnitPrice(ProductType.Laptop)).Returns(500m);

        var client = new IndividualCustomer("TEST001", "Test", "User");
        var cart = new Cart(client, mockStrategy.Object);

        // Act
        cart.AddProduct(ProductType.HighEndPhone, 1);
        cart.AddProduct(ProductType.Laptop, 2);

        // Assert
        cart.CalculateTotal().Should().Be(2000m); // 1000 + 2*500

        // Verify - un appel par type de produit
        mockStrategy.Verify(s => s.GetUnitPrice(ProductType.HighEndPhone), Times.Once);
        mockStrategy.Verify(s => s.GetUnitPrice(ProductType.Laptop), Times.Once);
        mockStrategy.VerifyNoOtherCalls();
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
        mockStrategy.Setup(s => s.GetUnitPrice(ProductType.MidRangePhone)).Returns(100m);

        var client = new IndividualCustomer("TEST", "Test", "User");
        var cart = new Cart(client, mockStrategy.Object);

        // Act
        cart.AddProduct(ProductType.MidRangePhone, 2);
        cart.AddProduct(ProductType.MidRangePhone, 3);

        // Assert
        cart.CalculateTotal().Should().Be(500m); // 5 * 100
    }
}