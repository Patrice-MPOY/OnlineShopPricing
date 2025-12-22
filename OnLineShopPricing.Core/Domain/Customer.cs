namespace OnlineShopPricing.Core.Domain
{
    public abstract class Customer(string customerId)
    {
        public string CustomerId { get; } = customerId ?? throw new ArgumentNullException(nameof(customerId));
    }
}
