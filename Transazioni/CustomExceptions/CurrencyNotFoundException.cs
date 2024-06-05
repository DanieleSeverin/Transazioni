namespace Transazioni.Domain.CustomExceptions;

public class CurrencyNotFoundException : Exception
{
    public CurrencyNotFoundException(string CurrencyCode) 
        : base($"The currency code '{CurrencyCode}' is invalid.") { }
}
