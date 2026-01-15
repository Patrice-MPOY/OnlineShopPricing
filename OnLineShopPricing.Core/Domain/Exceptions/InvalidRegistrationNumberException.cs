

namespace OnlineShopPricing.Core.Domain.Exceptions
{    public sealed class InvalidRegistrationNumberException : DomainException
    {
        public InvalidRegistrationNumberException()
            : base("Registration number is required.") { }
    }
    
}
