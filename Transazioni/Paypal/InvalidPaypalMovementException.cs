namespace Transazioni.Domain.Paypal;

public class InvalidPaypalMovementException : Exception
{
    public InvalidPaypalMovementException() 
        : base("Invalid Paypal Movement.")
    {
    }

    public InvalidPaypalMovementException(string message) 
        : base(message)
    {
    }

    public InvalidPaypalMovementException(string message, Exception innerException) 
        : base(message, innerException)
    {
    }
}
