namespace OnlineShopPricing.Core.Domain
{
    public abstract class Customer(string clientId)
    {
        public string ClientId { get; } = clientId ?? throw new ArgumentNullException(nameof(clientId));
    }
}
