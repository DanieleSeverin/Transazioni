namespace Transazioni.Application.Reporting.GetAccountsBalance;

public record AccountsBalanceSummary(Guid AccountId, string AccountName, decimal Balance, string Currency);