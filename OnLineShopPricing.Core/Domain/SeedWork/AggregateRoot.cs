using OnlineShopPricing.Core.Domain.Events;

namespace OnlineShopPricing.Core.Domain.SeedWork
{
    /// <summary>
    /// Base class for all Aggregate Roots in the domain.
    /// An Aggregate Root is the entry point to an aggregate and is responsible for maintaining its consistency.
    /// It exposes domain events that occurred within the aggregate.
    /// </summary>
    /// <typeparam name="TKey">The type of the aggregate's unique identifier (typically Guid, int, or a strong-typed ID).</typeparam>
    public abstract class AggregateRoot<TKey> : Entity<TKey>, IAggregateRoot
        where TKey : notnull, IEquatable<TKey>
    {
        // Private mutable list to collect domain events raised by the aggregate
        private readonly List<IDomainEvent> _domainEvents = new();

        /// <summary>
        /// Read-only view of the domain events that have occurred in this aggregate.
        /// Used by the infrastructure (e.g. application services, EF Core interceptors) to publish events.
        /// </summary>
        public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

        /// <summary>
        /// Protected constructor to enforce that derived classes provide an ID.
        /// </summary>
        /// <param name="id">The unique identifier of the aggregate.</param>
        protected AggregateRoot(TKey id) : base(id)
        {
        }

        /// <summary>
        /// Protected method to add a domain event from within the aggregate's business methods.
        /// Should be called when a significant state change occurs (e.g., product added, quantity changed).
        /// </summary>
        /// <param name="domainEvent">The domain event to raise.</param>
        /// <exception cref="ArgumentNullException">Thrown if domainEvent is null.</exception>
        protected void AddDomainEvent(IDomainEvent domainEvent)
        {
            if (domainEvent == null)
            {
                throw new ArgumentNullException(nameof(domainEvent));
            }

            _domainEvents.Add(domainEvent);
        }

        /// <summary>
        /// Clears all domain events from the aggregate.
        /// Called by the infrastructure after events have been published / dispatched.
        /// </summary>
        public void ClearDomainEvents()
        {
            _domainEvents.Clear();
        }

        /// <summary>
        /// Returns the current list of domain events and immediately clears the internal collection.
        /// This is the most common pattern used in EF Core interceptors or application services.
        /// </summary>
        /// <returns>A read-only copy of the domain events that were present.</returns>
        public IReadOnlyCollection<IDomainEvent> PopDomainEvents()
        {
            var events = _domainEvents.ToList();
            _domainEvents.Clear();
            return events;
        }
    }
}