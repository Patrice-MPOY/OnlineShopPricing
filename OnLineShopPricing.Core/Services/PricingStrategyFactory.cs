using OnlineShopPricing.Core.Domain;

namespace OnlineShopPricing.Core.Services
{
    /// <summary>
    /// Factory to create the appropriate pricing strategy based on customer type.
    /// Manual instantiation is used for clarity in this exercise.
    /// In a real application, strategies would be registered in the DI container and resolved automatically
    /// </summary>
    public class PricingStrategyFactory
    {
        public PricingStrategyFactory()
        {
        }
        public IPricingStrategy CreateStrategy(Customer client)
        {
            if (client == null) throw new ArgumentNullException(nameof(client));
            return client switch
            {
                IndividualCustomer => new IndividualPricingStrategy(),
                BusinessCustomer business when business.IsLargeAccount => new LargeBusinessPricingStrategy(),
                BusinessCustomer => new SmallBusinessPricingStrategy(),
                _ => throw new ArgumentException("Unsupported client type")
            };
        }
    }
}
