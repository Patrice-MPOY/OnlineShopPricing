using OnlineShopPricing.Core.Domain;
using OnlineShopPricing.Core.Resources;

namespace OnlineShopPricing.Core.Services
{
    /// <summary>
    /// Factory to create the appropriate pricing strategy based on customer type.
    /// Manual instantiation is used for clarity in this exercise.
    /// In a real application strategies would be registered in the DI container and automatically resolved 
    /// </summary>
    public static class PricingStrategyFactory
    {
        public static IPricingStrategy CreateStrategy(Customer customer)
        {
            ArgumentNullException.ThrowIfNull(customer);

            return customer switch
            {
                IndividualCustomer => new IndividualPricingStrategy(),
                BusinessCustomer business when business.IsLargeAccount => new LargeBusinessPricingStrategy(),
                BusinessCustomer => new SmallBusinessPricingStrategy(),
                _ => throw new ArgumentException(ErrorMessages.InvalidCustomerType, nameof(customer))
            };
        }
    }
}
