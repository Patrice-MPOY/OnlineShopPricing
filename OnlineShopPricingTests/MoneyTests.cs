using FluentAssertions;
using OnlineShopPricing.Core.Domain.Exceptions;
using OnlineShopPricing.Core.Domain.ValueObjects;
using Xunit;

namespace OnlineShopPricingTests
{
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
        public void Constructor_WhenAmountIsNegative_ShouldThrowInvalidAmountException(decimal negative)
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
        public void Constructor_WhenAmountIsNonNegative_ShouldCreateValidInstance(decimal valid)
        {
            var money = new Money(valid);
            money.Amount.Should().Be(valid);
        }

        [Fact]
        public void Constructor_WithVerySmallPositiveValue_ShouldAcceptIt()
        {
            var money = new Money(VerySmall);
            money.Amount.Should().Be(VerySmall);
        }

        [Fact]
        public void Constructor_WithNearMaximumDecimalValue_ShouldAcceptIt()
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
        public void Addition_OfTwoMoneyInstances_ShouldReturnCorrectSumAndNewInstance(
            decimal a, decimal b, decimal expected)
        {
            var left = new Money(a);
            var right = new Money(b);

            var result = left + right;

            result.Amount.Should().Be(expected);
            result.Should().NotBeSameAs(left);
            result.Should().NotBeSameAs(right);
        }

        [Fact]
        public void Addition_WithNearMaximumDecimal_ShouldNotOverflow()
        {
            var result = new Money(NearMaxDecimal) + new Money(1m);
            result.Amount.Should().Be(NearMaxDecimal + 1m);
        }

        // ──────────────────────────────────────────────────────────────────────
        // Equality & identity
        // ──────────────────────────────────────────────────────────────────────

        [Fact]
        public void Equality_TwoIdenticalMoneyInstances_ShouldBeEqual()
        {
            var a = new Money(42.50m);
            var b = new Money(42.50m);

            a.Should().Be(b);
            (a == b).Should().BeTrue();
            a.Equals(b).Should().BeTrue();
            a.GetHashCode().Should().Be(b.GetHashCode());
        }

        [Fact]
        public void Zero_ShouldAlwaysBeZeroAndImmutable()
        {
            Money.Zero.Amount.Should().Be(0m);

            var result = Money.Zero + new Money(10m);

            Money.Zero.Amount.Should().Be(0m);
            result.Amount.Should().Be(10m);
        }

        // ──────────────────────────────────────────────────────────────────────
        // Operator validations
        // ──────────────────────────────────────────────────────────────────────

        [Theory]
        [InlineData(-1)]
        [InlineData(-5)]
        [InlineData(int.MinValue)]
        public void Multiplication_ByNegativeQuantity_ShouldThrowInvalidQuantityException(int negativeQuantity)
        {
            var money = new Money(10m);

            var action = () => money * negativeQuantity;

            action.Should().ThrowExactly<InvalidQuantityException>()
                  .Which.Quantity.Should().Be(negativeQuantity);
        }
    }
}