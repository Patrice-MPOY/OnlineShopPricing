using OnlineShopPricing.Core.Domain;
using OnlineShopPricing.Core.Services;

namespace OnlineShopPricingTests.TestHelpers;
public class InvalidCustomerType() : Customer("UNSUPPORTED001")
{
    public override IPricingStrategy GetPricingStrategy()
    {
        throw new NotImplementedException();
    }
}


