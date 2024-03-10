using Transazioni.Application.Abstractions.Messaging;
using Transazioni.Domain.Movement;

namespace Transazioni.Application.Movement.CreateMovement;

public sealed record CreateMovementCommand(CreateMovementRequest Request) : ICommand<Movements>;
