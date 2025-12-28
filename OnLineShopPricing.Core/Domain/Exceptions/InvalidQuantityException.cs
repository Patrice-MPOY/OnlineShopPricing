using OnlineShopPricing.Core.Resources;

namespace OnlineShopPricing.Core.Domain.Exceptions
{
    public class InvalidQuantityException(int quantity) : DomainException(string.Format(ErrorMessages.QuantityMustBePositive, quantity))
    {
        public int Quantity { get; } = quantity;
    }
}

