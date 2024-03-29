using Transazioni.Application.Abstractions.Messaging;
using Transazioni.Domain.Utilities.Ordering;
using Transazioni.Domain.Utilities.Pagination;

namespace Transazioni.Application.Movement.GetMovements;

public sealed record GetMovementsQuery(
    GetMovementFilter filter, 
    PaginationConfigurations paginationConfigurations,
    OrderingConfigurations orderingConfigurations) 
        : IQuery<PaginationResponse<GetMovementsResponse>>;