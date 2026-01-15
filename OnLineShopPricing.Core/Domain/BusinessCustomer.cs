using OnlineShopPricing.Core.Domain.Exceptions;
using OnlineShopPricing.Core.Services;

namespace OnlineShopPricing.Core.Domain
{
    public sealed class BusinessCustomer(
        string id,
        string companyName,
        string? vatNumber,
        string registrationNumber,
        decimal annualTurnover)
        : Customer(id)
    {
        public string CompanyName { get; } =
            !string.IsNullOrWhiteSpace(companyName)
                ? companyName
                : throw new InvalidCompanyNameException();

        public string? VatNumber { get; } = vatNumber;

        public string RegistrationNumber { get; } =
            !string.IsNullOrWhiteSpace(registrationNumber)
                ? registrationNumber
                : throw new InvalidRegistrationNumberException();

        public decimal AnnualTurnover { get; } =
            annualTurnover >= 0
                ? annualTurnover
                : throw new InvalidAnnualTurnoverException(annualTurnover);

        public override IPricingStrategy GetPricingStrategy()
        {
            return AnnualTurnover > 10_000_000m
                ? new LargeBusinessPricingStrategy()
                : new SmallBusinessPricingStrategy();
        }
    }
}