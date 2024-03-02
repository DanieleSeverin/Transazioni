namespace Transazioni.Domain.Movement;

public record MovementId(Guid Value)
{
    public static MovementId New() => new(Guid.NewGuid());
}
