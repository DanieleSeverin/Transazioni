using Transazioni.Application.Abstractions.Messaging;
using Transazioni.Domain.Abstractions;

namespace Transazioni.Application.Reporting.GetCosts;

public class GetCostsQueryHandler : IQueryHandler<GetCostsQuery, List<CostsSummary>>
{
    private readonly ICostsProvider _costsProvider;

    public GetCostsQueryHandler(ICostsProvider costsProvider)
    {
        _costsProvider = costsProvider;
    }

    public async Task<Result<List<CostsSummary>>> Handle(GetCostsQuery request, CancellationToken cancellationToken)
    {
        return await _costsProvider.GetCosts(cancellationToken);
    }
}
