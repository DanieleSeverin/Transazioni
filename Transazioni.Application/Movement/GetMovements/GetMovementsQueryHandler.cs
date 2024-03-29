using Transazioni.Application.Abstractions.Messaging;
using Transazioni.Domain.Abstractions;
using Transazioni.Domain.Movement;
using Transazioni.Domain.Utilities.Pagination;
using Transazioni.Domain.Utilities.Ordering;

namespace Transazioni.Application.Movement.GetMovements;

public class GetMovementsQueryHandler : IQueryHandler<GetMovementsQuery, PaginationResponse<GetMovementsResponse>>
{
    public readonly IMovementsRepository _movementsRepository;

    public GetMovementsQueryHandler(IMovementsRepository movementsRepository)
    {
        _movementsRepository = movementsRepository;
    }

    public async Task<Result<PaginationResponse<GetMovementsResponse>>> Handle(GetMovementsQuery request, CancellationToken cancellationToken)
    {
        List<GetMovementsResponse> movements = (await _movementsRepository.Get(cancellationToken))
            .FilterByOriginAccountId(request.filter.originAccountId)
            .FilterByDestinationAccountId(request.filter.destinationAccountId)
            .GreaterOrEqualsThanDate(request.filter.startDate)
            .LowerOrEqualsThanDate(request.filter.endDate)
            .FilterByCategory(request.filter.category)
            .GreaterOrEqualsThanAmount(request.filter.amountGreaterThan)
            .LowerOrEqualsThanAmount(request.filter.amountLowerThan)
            .FilterByCurrency(request.filter.currency)
            .FilterByIsImported(request.filter.imported)
            .OrderByProperty(request.orderingConfigurations)
            .OutputCount(out int count)
            .Paginate(request.paginationConfigurations)
            .ToDto();

        return new PaginationResponse<GetMovementsResponse>(count, movements);
    }
}
