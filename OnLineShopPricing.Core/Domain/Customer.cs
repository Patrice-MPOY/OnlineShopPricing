using OnlineShopPricing.Core.Domain.Exceptions;
using OnlineShopPricing.Core.Services;

namespace OnlineShopPricing.Core.Domain
{
    /// <summary>
    /// Represents a customer in the pricing system.
    /// The customer is responsible for determining its own applicable pricing strategy 
    /// through polymorphic behavior (Tell, Don't Ask principle), making the domain more cohesive and extensible.
    /// </summary>
    public abstract class Customer(string customerId)
        {
            public string CustomerId { get; } = customerId
                                                ?? throw new MissingCustomerIdException(nameof(customerId));

        /// <summary>
        /// Returns the pricing strategy applicable to this customer.
        /// The concrete customer subclass is responsible for providing its specific implementation.
        /// </summary>
        /// <returns>An instance of IPricingStrategy suited to the customer type.</returns>
        public abstract IPricingStrategy GetPricingStrategy();
        }
}



