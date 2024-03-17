namespace Transazioni.Application.Reporting.GetRevenue;

public record RevenueSummary(Guid DestinationAccountId, string AccountName, decimal Amount, string Currency);
