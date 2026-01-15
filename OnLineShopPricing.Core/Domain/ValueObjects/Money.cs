using OnlineShopPricing.Core.Domain.Exceptions;
namespace OnlineShopPricing.Core.Domain.ValueObjects;

/// <summary>
/// Amount in euros. Immutable Value Object.
/// Cannot be negative. Currency is implicitly EUR.
/// </summary>
public readonly record struct Money
{
    /// <summary>
    /// Amount in euros
    /// </summary>
    public decimal Amount { get; }

    /// <summary>
    /// Creates a new Money instance with validation.
    /// </summary>
    /// <param name="amount">Amount in euros (must be ≥ 0)</param>
    /// <exception cref="InvalidAmountException"></exception>
    public Money(decimal amount)
    {
        if (amount < 0m)
            throw new InvalidAmountException("The amount cannot be negative.");

        Amount = amount;
    }

    /// <summary>
    /// Reusable instance representing zero euros
    /// </summary>
    public static Money Zero { get; } = new Money(0m);

    /// <summary>
    /// Addition of two Money values
    /// </summary>
    public static Money operator +(Money left, Money right)
        => new Money(left.Amount + right.Amount);

    /// <summary>
    /// Multiplication by a non-negative integer quantity
    /// </summary>
    public static Money operator *(Money money, int quantity)
        => quantity < 0
            ? throw new InvalidQuantityException(quantity)
            : new Money(money.Amount * quantity);
    public override string ToString() => $"{Amount:F2} €";
}