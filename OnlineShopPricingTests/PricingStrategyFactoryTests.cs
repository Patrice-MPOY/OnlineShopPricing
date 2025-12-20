using FluentAssertions;
using OnlineShopPricing.Core.Domain;
using OnlineShopPricing.Core.Services;

namespace OnlineShopPricingTests
{
    public class PricingStrategyFactoryTests
    {
        private readonly PricingStrategyFactory _factory = new();
        [Fact]
        public void Factory_Returns_IndividualStrategy_For_IndividualCustomer()
        {
            var client = new IndividualCustomer("1", "John", "Doe");
            var strategy = _factory.CreateStrategy(client);
            strategy.Should().BeOfType<IndividualPricingStrategy>();
        }

        [Fact]
        public void Factory_Returns_SmallBusinessStrategy_For_SmallBusinessCustomer()
        {
            var client = new BusinessCustomer("2", "Small Corp", "123", 5_000_000m);
            var strategy = _factory.CreateStrategy(client);
            strategy.Should().BeOfType<SmallBusinessPricingStrategy>();
        }

        [Fact]
        public void Factory_Returns_LargeBusinessStrategy_For_LargeBusinessCustomer()
        {
            var client = new BusinessCustomer("3", "Large Corp", "456", 15_000_000m);
            var strategy = _factory.CreateStrategy(client);
            strategy.Should().BeOfType<LargeBusinessPricingStrategy>();
        }
    }

}
