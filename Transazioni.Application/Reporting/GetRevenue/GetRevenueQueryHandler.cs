using Transazioni.Application.Abstractions.Messaging;
using Transazioni.Domain.Abstractions;

namespace Transazioni.Application.Reporting.GetRevenue;

public class GetRevenueQueryHandler : IQueryHandler<GetRevenueQuery, List<RevenueSummary>>
{
    private readonly IRevenueProvider _RevenueProvider;

    public GetRevenueQueryHandler(IRevenueProvider RevenueProvider)
    {
        _RevenueProvider = RevenueProvider;
    }

    public async Task<Result<List<RevenueSummary>>> Handle(GetRevenueQuery request, CancellationToken cancellationToken)
    {
        return await _RevenueProvider.GetRevenue(cancellationToken);
    }
}
