namespace Transazioni.Domain.Movement;

public static class MovementsFilters
{
    public static List<Movements> FilterByOriginAccountId(this List<Movements> movements, Guid? originAccountId)
    {
        if (originAccountId is null) return movements;

        return movements.Where(x => x.AccountId.Value == originAccountId).ToList();
    }

    public static List<Movements> FilterByDestinationAccountId(this List<Movements> movements, Guid? destinationAccountId)
    {
        if (destinationAccountId is null) return movements;

        return movements.Where(x => x.AccountId.Value == destinationAccountId).ToList();
    }

    public static List<Movements> GreaterOrEqualsThanDate(this List<Movements> movements, DateTime? date)
    {
        if (date is null) return movements;

        return movements.Where(x => x.Date >= date).ToList();
    }

    public static List<Movements> LowerOrEqualsThanDate(this List<Movements> movements, DateTime? date)
    {
        if (date is null) return movements;

        return movements.Where(x => x.Date <= date).ToList();
    }

    public static List<Movements> FilterByCategory(this List<Movements> movements, string? category)
    {
        if (string.IsNullOrWhiteSpace(category)) return movements.Where(x => x.Category is null).ToList();

        return movements.Where(x => 
            x.Category is not null &&
            x.Category.Value.Equals(category, StringComparison.InvariantCultureIgnoreCase) ).ToList();
    }

    public static List<Movements> GreaterOrEqualsThanAmount(this List<Movements> movements, decimal? amount)
    {
        if (amount is null) return movements;

        return movements.Where(x => x.Money.Amount >= amount).ToList();
    }

    public static List<Movements> LowerOrEqualsThanAmount(this List<Movements> movements, decimal? amount)
    {
        if (amount is null) return movements;

        return movements.Where(x => x.Money.Amount <= amount).ToList();
    }

    public static List<Movements> FilterByCurrency(this List<Movements> movements, string? currency)
    {
        if (string.IsNullOrWhiteSpace(currency)) return movements;

        return movements.Where(x =>
            x.Money.Currency.Code.Equals(currency, StringComparison.InvariantCultureIgnoreCase)).ToList();
    }

    public static List<Movements> FilterByIsImported(this List<Movements> movements, bool? isImported)
    {
        if (isImported is null) return movements;

        return movements.Where(x => x.IsImported == isImported).ToList();
    }
}
