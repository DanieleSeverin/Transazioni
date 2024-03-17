namespace Transazioni.Application.Reporting.GetCosts;

public record CostsSummary(Guid DestinationAccountId, string AccountName, decimal Amount, string Currency);
