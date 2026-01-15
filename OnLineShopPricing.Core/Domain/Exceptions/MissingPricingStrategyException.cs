

namespace OnlineShopPricing.Core.Domain.Exceptions
{
    public sealed class MissingPricingStrategyException : DomainException
    {
        public MissingPricingStrategyException()
            : base("Customer has no applicable pricing strategy.") { }
    }

}
