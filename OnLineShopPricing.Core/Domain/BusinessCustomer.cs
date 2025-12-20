namespace OnlineShopPricing.Core.Domain
{
    public class BusinessCustomer(
        string clientId,
        string companyName,
        string registrationNumber,
        decimal annualRevenue,
        string? vatNumber = null) : Customer(clientId)
    {
        public string CompanyName { get; } = companyName ?? throw new ArgumentNullException(nameof(companyName));
        public string? VatNumber { get; } = vatNumber;
        public string RegistrationNumber { get; } = registrationNumber ?? throw new ArgumentNullException(nameof(registrationNumber));
        public decimal AnnualRevenue { get; } = annualRevenue;
        public bool IsLargeAccount => AnnualRevenue > 10_000_000m;
    }
}
