using OnlineShopPricing.Core.Resources;

namespace OnlineShopPricing.Core.Domain.Exceptions
{
    public class InvalidProductTypeException : DomainException
    {
        public string ProductType { get; }

        public InvalidProductTypeException(string productType)
            : base(string.Format(ErrorMessages.InvalidProductType, productType))
        {
            ProductType = productType;
        }
    }
}

