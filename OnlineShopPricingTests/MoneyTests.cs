using FluentAssertions;
using OnlineShopPricing.Core.Domain.Exceptions;
using OnlineShopPricing.Core.Domain.ValueObjects;


namespace OnlineShopPricingTests ;

public class MoneyTests
{
    // Ces constantes restent utilisables dans les tests normaux (Fact / Theory)
    private const decimal VerySmall = 0.0000000000000000000000000001m;
    private const decimal NearMaxDecimal = 7922816251426433759354395033.5m;

    // ──────────────────────────────────────────────────────────────────────
    // Creation & basic invariants
    // ──────────────────────────────────────────────────────────────────────

    [Theory]
    [InlineData(-0.0001)]
    [InlineData(-1)]
    [InlineData(-999_999.99)]
    public void Constructor_Should_Reject_Negative_Amounts(decimal negative)
    {
        var action = () => new Money(negative);

        action.Should().ThrowExactly<InvalidAmountException>()
              .WithMessage("*cannot be negative*");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(0.0001)]
    [InlineData(1)]
    [InlineData(42.50)]
    [InlineData(999_999.99)]
    public void Constructor_Should_Accept_Common_NonNegative_Values(decimal valid)
    {
        var money = new Money(valid);
        money.Amount.Should().Be(valid);
    }

    // Cas extrêmes → on passe en Fact dédiés pour éviter CS0182
    [Fact]
    public void Constructor_Should_Accept_Very_Small_Positive_Value()
    {
        var money = new Money(VerySmall);
        money.Amount.Should().Be(VerySmall);
    }

    [Fact]
    public void Constructor_Should_Accept_Near_Maximum_Decimal_Value()
    {
        var money = new Money(NearMaxDecimal);
        money.Amount.Should().Be(NearMaxDecimal);
    }

    // ──────────────────────────────────────────────────────────────────────
    // Arithmetic operations 
    // ──────────────────────────────────────────────────────────────────────

    [Theory]
    [InlineData(10.50, 20.25, 30.75)]
    [InlineData(0, 999_999.99, 999_999.99)]
    public void Addition_Should_Be_Correct_And_Create_New_Instance(
        decimal a, decimal b, decimal expected)
    {
        var left = new Money(a);
        var right = new Money(b);

        var result = left + right;

        result.Amount.Should().Be(expected);
        result.Should().NotBeSameAs(left);
        result.Should().NotBeSameAs(right);
    }
    
    // Cas extrême en addition (exemple)
    [Fact]
    public void Addition_Should_Handle_Near_Max_Decimal_Without_Overflow()
    {
        var result = new Money(NearMaxDecimal) + new Money(1m);
        result.Amount.Should().Be(NearMaxDecimal + 1m);
    }

    [Fact]
    public void Two_Identical_Money_Should_Be_Equal()
    {
        var a = new Money(42.50m);
        var b = new Money(42.50m);

        a.Should().Be(b);                    
        (a == b).Should().BeTrue();          
        a.Equals(b).Should().BeTrue();
        a.GetHashCode().Should().Be(b.GetHashCode());
    }

    [Fact]
    public void Zero_Should_Always_Be_Zero_And_Immutable()
    {
        Money.Zero.Amount.Should().Be(0m);

        var result = Money.Zero + new Money(10m);

        Money.Zero.Amount.Should().Be(0m);
        result.Amount.Should().Be(10m);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(-5)]
    [InlineData(int.MinValue)]
    public void Multiplication_By_Negative_Quantity_Should_Throw_InvalidQuantityException(int negativeQuantity)
    {
        var money = new Money(10m);

        var action = () => money * negativeQuantity;

        action.Should().ThrowExactly<InvalidQuantityException>()
            .Which.Quantity.Should().Be(negativeQuantity);
    }


}