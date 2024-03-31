using Transazioni.Domain.Movement;

namespace Transazioni.Domain.Shared;

public record Money(decimal Amount, Currency Currency) : IComparable, IComparable<Money>
{
    public static Money operator +(Money first, Money second)
    {
        if (first.Currency != second.Currency)
        {
            throw new InvalidOperationException("Currencies have to be equal");
        }

        return new Money(first.Amount + second.Amount, first.Currency);
    }

    public static Money Zero() => new(0, Currency.None);

    public static Money Zero(Currency currency) => new(0, currency);

    public bool IsZero() => this == Zero(Currency);

    public int CompareTo(object? obj)
    {
        if (obj == null)
        {
            return 1;
        }

        if (obj.GetType() != typeof(Money))
        {
            throw new ArgumentException("Argoument type should be 'Money'.");
        }

        return this.CompareTo((Money?)obj);
    }

    public int CompareTo(Money? other)
    {
        if (other == null)
        {
            return 1;
        }

        if (this.Currency.Code != other.Currency.Code)
        {
            return this.Currency.Code.CompareTo(other.Currency.Code);
        }

        return this.Amount.CompareTo(other.Amount);
    }
}
