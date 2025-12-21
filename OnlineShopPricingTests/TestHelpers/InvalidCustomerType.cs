
using OnlineShopPricing.Core.Domain;


namespace OnlineShopPricing.Tests.TestHelpers;
internal class InvalidCustomerType : Customer
{
    public InvalidCustomerType() : base("UNSUPPORTED001") { }
}


