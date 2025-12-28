using OnlineShopPricing.Core.Resources;

namespace OnlineShopPricing.Core.Domain.Exceptions
{
    public class InvalidProductTypeException(string productType) : DomainException(string.Format(ErrorMessages.InvalidProductType, productType))
    {
        public string ProductType { get; } = productType;
    }
}

