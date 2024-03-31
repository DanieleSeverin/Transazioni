namespace Transazioni.Domain.Movement;

public record MovementDescription(string Value) : IComparable, IComparable<MovementDescription>
{
    public int CompareTo(object? obj)
    {
        if (obj == null)
        {
            return 1;
        }

        if (obj.GetType() != typeof(MovementDescription))
        {
            throw new ArgumentException("Argoument type should be 'MovementDescription'.");
        }

        return this.CompareTo((MovementDescription?)obj);
    }

    public int CompareTo(MovementDescription? other)
    {
        if (other == null)
        {
            return 1;
        }

        return this.Value.CompareTo(other.Value);
    }
}
