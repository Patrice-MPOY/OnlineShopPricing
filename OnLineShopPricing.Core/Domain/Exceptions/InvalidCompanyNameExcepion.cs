

namespace OnlineShopPricing.Core.Domain.Exceptions
{
    public sealed class InvalidCompanyNameException : DomainException
    {
        public InvalidCompanyNameException()
            : base("Company name is required.") { }
    }

   
}

