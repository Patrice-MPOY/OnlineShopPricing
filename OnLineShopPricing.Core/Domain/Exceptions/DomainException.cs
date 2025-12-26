
using System;

namespace OnlineShopPricing.Core.Domain.Exceptions
{
    /// <summary>
    /// Base exception for all domain rule violations.
    /// Explicitly represents a domain-level error.
    /// </summary>
    public abstract class DomainException : Exception
    {
        protected DomainException(string message)
            : base(message)
        {
        }

        protected DomainException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
