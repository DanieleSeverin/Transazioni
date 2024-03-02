namespace Transazioni.Domain.CheBanca;

public class InvalidCheBancaMovementException : Exception
{
    public InvalidCheBancaMovementException() 
        : base("Invalid CheBanca Movement.")
    {
    }

    public InvalidCheBancaMovementException(string message) 
        : base(message)
    {
    }

    public InvalidCheBancaMovementException(string message, Exception innerException) 
        : base(message, innerException)
    {
    }
}
