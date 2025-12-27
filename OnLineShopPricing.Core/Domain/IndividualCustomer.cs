using OnlineShopPricing.Core.Services;
namespace OnlineShopPricing.Core.Domain
{
    public sealed class IndividualCustomer(string customerId, string firstName, string lastName)
        : Customer(customerId)
    {
        public string FirstName { get; } = firstName ?? throw new ArgumentNullException(nameof(firstName));
        public string LastName { get; } = lastName ?? throw new ArgumentNullException(nameof(lastName));

        public override IPricingStrategy GetPricingStrategy() => new IndividualPricingStrategy();
    }
}
