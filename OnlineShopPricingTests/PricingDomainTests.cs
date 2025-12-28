using FluentAssertions;
using OnlineShopPricing.Core.Domain;

namespace OnlineShopPricingTests
{
    /// <summary>
    /// These tests validate pricing behavior using real pricing strategy implementations.
    /// They are written to remain readable and deterministic while covering the core
    /// pricing rules of the system.
    ///
    /// The pricing strategy is now resolved polymorphically via Customer.GetPricingStrategy(),
    /// making the design more domain-centric, extensible, and aligned with SOLID principles.
    /// </summary>
    public class PricingDomainTests
    {
        // Shared test data for parameterized pricing tests - unchanged
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

            // Large business pricing (annual turnover > 10M)
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
            },

            // Business pricing (annual turnover exactly 10M) → treated as SmallBusiness
            new object[]
            {
                new BusinessCustomer("BIZ010", "Exact Corp", "REG010", 10_000_000m),
                new (ProductType Product, int Quantity)[]
                {
                    (ProductType.HighEndPhone, 1)
                },
                1150m
            }
        };

        [Theory]
        [MemberData(nameof(CalculateTotalTestData))]
        public void CalculateTotal_WhenCustomerTypeIsDifferent_ShouldApplyCorrectPricing(
            Customer customer,
            (ProductType Product, int Quantity)[] cartItems,
            decimal expectedTotal)
        {
            // Arrange
            var cart = new Cart(customer); // Strategy resolved polymorphically inside Cart

            // Act
            foreach (var (product, quantity) in cartItems)
            {
                cart.AddProduct(product, quantity);
            }

            var total = cart.CalculateTotal();

            // Assert
            total.Should().Be(expectedTotal);
        }
        
        public static IEnumerable<object[]> ConsistentPriceTestData => new List<object[]>
        {
            new object[] { typeof(IndividualCustomer), (decimal?)null },
            new object[] { typeof(BusinessCustomer), 5_000_000m },   // Small business
            new object[] { typeof(BusinessCustomer), 15_000_000m }   // Large business
        };

        [Theory]
        [MemberData(nameof(ConsistentPriceTestData))]
        public void PricingStrategy_ReturnsConsistentPrice_ForGivenCustomerType(Type customerType, decimal? turnover)
        {
            // Arrange
            Customer customer = customerType switch
            {
                var t when t == typeof(IndividualCustomer)
                    => new IndividualCustomer("ID001", "John", "Doe"),
                var t when t == typeof(BusinessCustomer)
                    => new BusinessCustomer("BIZ001", "Corp", "REG123", turnover!.Value),
                _ => throw new NotSupportedException()
            };

            var pricingStrategy = customer.GetPricingStrategy();
            const ProductType product = ProductType.HighEndPhone;

            // Act
            var firstPrice = pricingStrategy.GetUnitPrice(product);
            var secondPrice = pricingStrategy.GetUnitPrice(product);

            // Assert
            firstPrice.Should().Be(secondPrice);
        }

    }
}