namespace Transazioni.Application.Movement.GetMovements;

public sealed record GetMovementsResponse(
    Guid Id,
    DateTime Date,
    string Description,
    decimal Amount,
    string Currency,
    MovementAccount OriginAccount,
    MovementAccount DestinationAccount,
    string? Category,
    bool IsImported,
    string Peridiocity
    );

public sealed record MovementAccount(
    Guid Id,
    string AccountName,
    bool IsPatrimonial);
