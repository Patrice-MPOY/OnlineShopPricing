using OnlineShopPricing.Core.Services;

namespace OnlineShopPricing.Core.Domain
{
    public sealed class BusinessCustomer(
        string id,
        string companyName,
        string registrationNumber,
        decimal annualTurnover) : Customer(id)
    {
        public string CompanyName { get; } = companyName;
        public string RegistrationNumber { get; } = registrationNumber;
        public decimal AnnualTurnover { get; } = annualTurnover;

        public override IPricingStrategy GetPricingStrategy()
        {
           return AnnualTurnover > 10_000_000m
                ? new LargeBusinessPricingStrategy()
                : new SmallBusinessPricingStrategy();
        }
    }
}
