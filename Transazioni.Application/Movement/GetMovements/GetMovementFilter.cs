namespace Transazioni.Application.Movement.GetMovements;

public sealed record GetMovementFilter(
    Guid? originAccountId,
    Guid? destinationAccountId,
    DateTime? startDate,
    DateTime? endDate,
    string? category,
    decimal? amountGreaterThan,
    decimal? amountLowerThan,
    string? currency,
    bool? imported
    );
