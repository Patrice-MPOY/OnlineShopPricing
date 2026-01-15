using OnlineShopPricing.Core.Domain.Exceptions;
using OnlineShopPricing.Core.Services;

namespace OnlineShopPricing.Core.Domain
{
    public sealed class IndividualCustomer(
        string customerId,
        string firstName,
        string lastName)
        : Customer(customerId)
    {
        public string FirstName { get; } =
            !string.IsNullOrWhiteSpace(firstName)
                ? firstName
                : throw new InvalidFirstNameException();

        public string LastName { get; } =
            !string.IsNullOrWhiteSpace(lastName)
                ? lastName
                : throw new InvalidLastNameException();

        public override IPricingStrategy GetPricingStrategy()
            => new IndividualPricingStrategy();
    }
}