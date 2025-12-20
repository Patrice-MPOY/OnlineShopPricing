namespace OnlineShopPricing.Core.Domain
{
    public class IndividualCustomer(string clientId, string firstName, string lastName) : Customer(clientId)
    {
        public string FirstName { get; } = firstName ?? throw new ArgumentNullException(nameof(firstName));
        public string LastName { get; } = lastName ?? throw new ArgumentNullException(nameof(lastName));
    }
}
