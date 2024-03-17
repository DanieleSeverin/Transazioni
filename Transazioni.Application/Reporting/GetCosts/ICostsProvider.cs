namespace Transazioni.Application.Reporting.GetCosts;

public interface ICostsProvider
{
    public abstract Task<List<CostsSummary>> GetCosts(CancellationToken cancellationToken);
}
