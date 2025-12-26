using OnlineShopPricing.Core.Resources;

namespace OnlineShopPricing.Core.Domain.Exceptions
{
    public class InvalidQuantityException : DomainException
    {
        public int Quantity { get; }
        public InvalidQuantityException(int quantity)
            : base(string.Format(ErrorMessages.QuantityMustBePositive, quantity))
        {
            Quantity = quantity;
        }
    }
}

