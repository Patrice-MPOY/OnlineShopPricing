
    using global::OnlineShopPricing.Core.Resources;
    using System;

    namespace OnlineShopPricing.Core.Domain.Exceptions
    {
        /// <summary>
        /// Représente une erreur lorsqu'un type de client n'est pas supporté 
        /// par la factory de stratégies de pricing.
        /// </summary>
        public sealed class InvalidCustomerTypeException : Exception
        {
            public InvalidCustomerTypeException()
                : base(ErrorMessages.InvalidCustomerType)
            {
            }

            public InvalidCustomerTypeException(string message)
                : base(message)
            {
            }

            public InvalidCustomerTypeException(string message, Exception innerException)
                : base(message, innerException)
            {
            }
        }
    }


    

