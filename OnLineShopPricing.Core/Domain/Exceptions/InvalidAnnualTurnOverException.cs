
namespace OnlineShopPricing.Core.Domain.Exceptions
{
    public sealed class InvalidAnnualTurnoverException : DomainException
    {
        public InvalidAnnualTurnoverException(decimal value)
            : base($"Annual turnover cannot be negative (value: {value}).") { }
    }
}
