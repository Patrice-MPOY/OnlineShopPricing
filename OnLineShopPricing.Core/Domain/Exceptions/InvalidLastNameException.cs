
namespace OnlineShopPricing.Core.Domain.Exceptions
{
    public sealed class InvalidLastNameException : DomainException
    {
        public InvalidLastNameException()
            : base("Last name is required.") { }
    }
}
