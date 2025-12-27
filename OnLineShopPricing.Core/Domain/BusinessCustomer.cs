using OnlineShopPricing.Core.Services;

namespace OnlineShopPricing.Core.Domain
{
    public sealed class BusinessCustomer : Customer
    {
        public string CompanyName { get; }
        public string RegistrationNumber { get; }
        public decimal AnnualTurnover { get; }
        public BusinessCustomer(
            string id,
            string companyName,
            string registrationNumber,
            decimal annualTurnover)
            : base(id)
        {
            CompanyName = companyName;
            RegistrationNumber = registrationNumber;
            AnnualTurnover = annualTurnover;
        }
        
        public override IPricingStrategy GetPricingStrategy()
        {
           return AnnualTurnover > 10_000_000m
                ? new LargeBusinessPricingStrategy()
                : new SmallBusinessPricingStrategy();
        }
    }
}
