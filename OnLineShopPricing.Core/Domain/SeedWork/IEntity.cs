using System.Diagnostics.CodeAnalysis;

namespace OnlineShopPricing.Core.Domain.SeedWork
{
    /// <summary>
    /// Base interface for all domain entities (DDD).
    /// </summary>
    /// <remarks>
    /// An entity is defined by a unique, immutable identity (Id).
    /// Equality is based solely on the Id, not on attribute values.
    ///
    /// This interface is kept minimal to focus on identity and transience.
    /// Domain events are intentionally not included here — prefer a separate
    /// marker interface (IHasDomainEvents) for aggregates that need them.
    /// </remarks>
    /// <typeparam name="K">The type of the entity's primary key (e.g. Guid, int, or strong ID type)</typeparam>
    public interface IEntity<K>
        where K : notnull, IEquatable<K>
    {
        /// <summary>
        /// Unique and immutable identifier of the entity.
        /// </summary>
        /// <remarks>
        /// Must be assigned at creation and never changed afterwards.
        /// Protected setter allows ORM (EF Core) to populate it during loading.
        /// </remarks>
        K Id { get; }

        /// <summary>
        /// Indicates whether the entity is transient (not yet persisted).
        /// </summary>
        /// <remarks>
        /// Returns true when the Id still has its default value (e.g. Guid.Empty or 0).
        /// Useful for EF Core change tracking and some domain logic.
        /// </remarks>
        bool IsTransient();
    }

   
}