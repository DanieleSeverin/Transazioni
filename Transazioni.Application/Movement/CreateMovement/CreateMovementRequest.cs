namespace Transazioni.Application.Movement.CreateMovement;

public sealed record CreateMovementRequest(Guid accountId, 
                                           Guid destinationAccountId, 
                                           DateTime date,
                                           string description,
                                           decimal amount,
                                           string currency,
                                           string? category,
                                           string periodicity);