using System;
using System.Diagnostics.CodeAnalysis;

namespace OnlineShopPricing.Core.Domain.SeedWork
{
    /// <summary>
    /// Abstract base class for all domain entities following Domain-Driven Design (DDD) principles.
    /// </summary>
    /// <remarks>
    /// An entity is defined by a unique, immutable identity (<see cref="Id"/>).
    /// Equality is based solely on the identity, not on attribute values.
    ///
    /// Key features:
    /// - Identity-based equality (DDD core principle)
    /// - Transient detection (<see cref="IsTransient"/>)
    /// - EF Core friendly: virtual Id with protected setter
    /// - Safe handling of transient entities during equality checks
    ///
    /// This class should be inherited by all domain entities (e.g., Cart, Order, Customer).
    /// </remarks>
    /// <typeparam name="TKey">The type of the entity's primary key (typically Guid, int, or a strong ID type)</typeparam>
    public abstract class Entity<TKey> : IEntity<TKey>
        where TKey : notnull, IEquatable<TKey>
    {
        /// <summary>
        /// Unique and immutable identifier of the entity.
        /// </summary>
        /// <remarks>
        /// - Must be assigned at creation and never modified afterwards.
        /// - Protected setter allows EF Core to populate it during materialization.
        /// - In domain code, treat it as read-only.
        /// </remarks>
        public TKey Id { get; protected set; } = default!;

        /// <summary>
        /// Protected constructor to enforce that derived classes provide an ID at creation.
        /// This ensures the identity is set immediately and remains immutable in domain logic.
        /// </summary>
        /// <param name="id">The unique identifier of the entity.</param>
        protected Entity(TKey id)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            Id = id;
        }

        /// <summary>
        /// Parameterless constructor for EF Core materialization.
        /// EF Core uses reflection to set the Id property after querying the database.
        /// </summary>
        protected Entity()
        {
        }

        /// <summary>
        /// Determines whether the entity is transient (not yet persisted).
        /// </summary>
        /// <remarks>
        /// Returns <c>true</c> if the identifier still has its default value
        /// (e.g., <c>Guid.Empty</c> or <c>0</c>).
        ///
        /// Useful for:
        /// - EF Core change tracking (new vs existing entities)
        /// - Preventing certain operations on non-persisted entities
        /// </remarks>
        /// <returns>true if the entity has not been assigned a persistent identifier yet</returns>
        public virtual bool IsTransient()
        {
            return EqualityComparer<TKey>.Default.Equals(Id, default);
        }

        // ──────────────────────────────────────────────────────────────
        // Identity-based equality (DDD core principle)
        // ──────────────────────────────────────────────────────────────

        /// <inheritdoc />
        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;

            return Equals((Entity<TKey>)obj);
        }

        /// <summary>
        /// Performs type-safe equality comparison based on the entity's identity.
        /// </summary>
        /// <remarks>
        /// Transient entities (Id = default) are compared by reference only.
        /// Persisted entities are compared by Id.
        /// </remarks>
        protected virtual bool Equals(Entity<TKey>? other)
        {
            if (other is null) return false;

            // Handle transient entities (before persistence)
            if (IsTransient() && other.IsTransient())
            {
                return ReferenceEquals(this, other);
            }

            return Id.Equals(other.Id);
        }

        /// <inheritdoc />
        [SuppressMessage("ReSharper", "NonReadonlyMemberInGetHashCode")]
        public override int GetHashCode()
        {
            // Once persisted, hash code is stable based on Id
            if (!IsTransient())
            {
                return Id.GetHashCode();
            }

            // For transient entities, fall back to reference-based hash
            return base.GetHashCode();
        }

        /// <summary>
        /// Equality operator based on entity identity.
        /// </summary>
        public static bool operator ==(Entity<TKey>? left, Entity<TKey>? right)
        {
            if (left is null) return right is null;
            return left.Equals(right);
        }

        /// <summary>
        /// Inequality operator based on entity identity.
        /// </summary>
        public static bool operator !=(Entity<TKey>? left, Entity<TKey>? right)
        {
            return !(left == right);
        }
    }
}