namespace Transazioni.Application.Reporting.AccountsBalance.GetAccountsBalance;

public record AccountsBalanceSummary(Guid AccountId, string AccountName, decimal Balance, string Currency);