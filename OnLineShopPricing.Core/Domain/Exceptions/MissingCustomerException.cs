using System;

namespace OnlineShopPricing.Core.Domain.Exceptions
{
    /// <summary>
    /// Thrown when a Cart is created or manipulated without an associated Customer.
    /// This violates the core domain invariant: "A cart always belongs to a valid Customer."
    /// </summary>
    public class MissingCustomerException : DomainException
    {
        /// <summary>
        /// The invalid (null) customer reference that caused the exception, if available.
        /// Useful for diagnostics and logging.
        /// </summary>
        public Customer? InvalidCustomer { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MissingCustomerException"/> class.
        /// </summary>
        public MissingCustomerException()
            : base("A cart cannot exist or be modified without a valid customer.")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MissingCustomerException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public MissingCustomerException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MissingCustomerException"/> class with a specified error message
        /// and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        public MissingCustomerException(string message, Exception? innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MissingCustomerException"/> class with the invalid customer reference.
        /// </summary>
        /// <param name="invalidCustomer">The null or invalid customer that caused the violation.</param>
        public MissingCustomerException(Customer? invalidCustomer)
            : base("A cart cannot exist or be modified without a valid customer.")
        {
            InvalidCustomer = invalidCustomer;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MissingCustomerException"/> class with a specified error message
        /// and the invalid customer reference.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="invalidCustomer">The null or invalid customer that caused the violation.</param>
        public MissingCustomerException(string message, Customer? invalidCustomer)
            : base(message)
        {
            InvalidCustomer = invalidCustomer;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MissingCustomerException"/> class with a specified error message,
        /// a reference to the inner exception, and the invalid customer reference.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        /// <param name="invalidCustomer">The null or invalid customer that caused the violation.</param>
        public MissingCustomerException(string message, Exception? innerException, Customer? invalidCustomer)
            : base(message, innerException)
        {
            InvalidCustomer = invalidCustomer;
        }
    }
}
