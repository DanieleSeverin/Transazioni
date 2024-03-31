namespace Transazioni.Domain.Movement;

public static class MovementsFilters
{
    public static IEnumerable<Movements> FilterByOriginAccountId(this IEnumerable<Movements> movements, Guid? originAccountId)
    {
        if (originAccountId is null) return movements;

        return movements.Where(x => x.AccountId.Value == originAccountId);
    }

    public static IEnumerable<Movements> FilterByDestinationAccountId(this IEnumerable<Movements> movements, Guid? destinationAccountId)
    {
        if (destinationAccountId is null) return movements;

        return movements.Where(x => x.DestinationAccountId.Value == destinationAccountId);
    }

    public static IEnumerable<Movements> GreaterOrEqualsThanDate(this IEnumerable<Movements> movements, DateTime? date)
    {
        if (date is null) return movements;

        return movements.Where(x => x.Date >= date);
    }

    public static IEnumerable<Movements> LowerOrEqualsThanDate(this IEnumerable<Movements> movements, DateTime? date)
    {
        if (date is null) return movements;

        return movements.Where(x => x.Date <= date);
    }

    public static IEnumerable<Movements> FilterByCategory(this IEnumerable<Movements> movements, string? category)
    {
        if (string.IsNullOrWhiteSpace(category)) return movements;

        return movements.Where(x => 
            x.Category is not null &&
            x.Category.Value.Equals(category, StringComparison.InvariantCultureIgnoreCase) );
    }

    public static IEnumerable<Movements> GreaterOrEqualsThanAmount(this IEnumerable<Movements> movements, decimal? amount)
    {
        if (amount is null) return movements;

        return movements.Where(x => x.Money.Amount >= amount);
    }

    public static IEnumerable<Movements> LowerOrEqualsThanAmount(this IEnumerable<Movements> movements, decimal? amount)
    {
        if (amount is null) return movements;

        return movements.Where(x => x.Money.Amount <= amount);
    }

    public static IEnumerable<Movements> FilterByCurrency(this IEnumerable<Movements> movements, string? currency)
    {
        if (string.IsNullOrWhiteSpace(currency)) return movements;

        return movements.Where(x =>
            x.Money.Currency.Code.Equals(currency, StringComparison.InvariantCultureIgnoreCase));
    }

    public static IEnumerable<Movements> FilterByIsImported(this IEnumerable<Movements> movements, bool? isImported)
    {
        if (isImported is null) return movements;

        return movements.Where(x => x.IsImported == isImported);
    }
}
