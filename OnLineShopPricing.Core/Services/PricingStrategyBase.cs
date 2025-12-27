using OnlineShopPricing.Core.Domain;
using System.Collections.Generic;
namespace OnlineShopPricing.Core.Services
{
        /// <summary>
        /// Base class for all pricing strategies in the system.
        /// Implements the common behavior of the <see cref="IPricingStrategy"/> interface
        /// by delegating the actual price data to concrete subclasses.
        /// 
        /// This design follows the Template Method pattern: the base class defines the algorithm
        /// (how to retrieve a price), while subclasses provide the data (the actual prices).
        /// </summary>
        public abstract class PricingStrategyBase : IPricingStrategy
        {
            /// <summary>
            /// Gets the read-only dictionary containing the unit prices for all known product types
            /// in this pricing strategy.
            /// 
            /// Each concrete strategy (Individual, Small Business, Large Business) provides its own
            /// price grid by overriding this property.
            /// </summary>
            protected abstract IReadOnlyDictionary<ProductType, decimal> Prices { get; }

            /// <summary>
            /// Tries to retrieve the unit price for the specified product type.
            /// </summary>
            /// <param name="product">The product type to look up.</param>
            /// <param name="price">
            /// When this method returns true, contains the unit price for the product.
            /// When this method returns false, the value is undefined.
            /// </param>
            /// <returns>
            /// true if the product type is known and has a defined price; otherwise, false.
            /// </returns>
            /// <remarks>
            /// This method is used by the Cart during AddProduct to validate that the product
            /// exists in the current pricing grid before accepting it.
            /// </remarks>
            public bool TryGetUnitPrice(ProductType product, out decimal price)
            {
                return Prices.TryGetValue(product, out price);
            }

            /// <summary>
            /// Gets the unit price for the specified product type.
            /// </summary>
            /// <param name="product">The product type to look up.</param>
            /// <returns>The unit price for the product.</returns>
            /// <exception cref="KeyNotFoundException">
            /// Thrown when the product type is not present in the pricing grid.
            /// This indicates either a programming error or an unsupported product.
            /// </exception>
            /// <remarks>
            /// This method is called by Cart.CalculateTotal() for each distinct product type
            /// to compute the line total (price × quantity).
            /// It uses the indexer for brevity, accepting the default KeyNotFoundException behavior.
            /// </remarks>
            public decimal GetUnitPrice(ProductType product)
            {
                return Prices[product];
            }
        }
}

