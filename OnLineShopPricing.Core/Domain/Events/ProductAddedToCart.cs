
using OnlineShopPricing.Core.Domain.ValueObjects;

namespace OnlineShopPricing.Core.Domain.Events
{
    public record ProductAddedToCart(
        Guid CartId,
        string CustomerId,
        ProductType Product,
        int QuantityAdded,
        int NewTotalQuantity,
        Money UnitPrice,
        DateTime OccurredOn
    ) : IDomainEvent
    {
        DateTime IDomainEvent.OccurredOn => OccurredOn;
    }
    
}
