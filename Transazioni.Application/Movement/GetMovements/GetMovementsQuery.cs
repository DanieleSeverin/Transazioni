using Transazioni.Application.Abstractions.Messaging;
using Transazioni.Domain.Movement;

namespace Transazioni.Application.Movement.GetMovements;

public sealed record GetMovementsQuery() : IQuery<List<GetMovementsResponse>>;