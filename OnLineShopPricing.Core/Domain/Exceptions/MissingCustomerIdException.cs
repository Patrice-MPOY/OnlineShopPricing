using System;

namespace OnlineShopPricing.Core.Domain.Exceptions
{
    /// <summary>
    /// Thrown when a Customer is created without a valid identifier.
    /// This violates the domain invariant: "Every customer must have a unique, non-empty identifier."
    /// </summary>
    public class MissingCustomerIdException : DomainException
    {
        public MissingCustomerIdException()
            : base("Customer identifier cannot be null or empty.")
        {
        }

        public MissingCustomerIdException(string message)
            : base(message)
        {
        }

        public MissingCustomerIdException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
