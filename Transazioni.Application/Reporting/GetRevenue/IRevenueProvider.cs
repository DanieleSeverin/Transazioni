namespace Transazioni.Application.Reporting.GetRevenue;

public interface IRevenueProvider
{
    public abstract Task<List<RevenueSummary>> GetRevenue(CancellationToken cancellationToken);
}
