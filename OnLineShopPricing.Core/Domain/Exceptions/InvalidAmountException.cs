namespace OnlineShopPricing.Core.Domain.Exceptions;

public class InvalidAmountException : DomainException
{
    public InvalidAmountException(string message)
        : base(message)
    {
    }

    public InvalidAmountException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    // Optionnel : ajouter des constructeurs avec montant pour plus de contexte
    public InvalidAmountException(decimal invalidAmount)
        : base($"Le montant {invalidAmount} est invalide (montant négatif interdit).")
    {
    }
}