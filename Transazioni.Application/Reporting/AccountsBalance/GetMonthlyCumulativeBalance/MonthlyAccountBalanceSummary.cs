namespace Transazioni.Application.Reporting.AccountsBalance.GetMonthlyCumulativeBalance;

public record MonthlyAccountBalanceSummary(
     Guid AccountId,
     string AccountName,
     DateTime Month,
     decimal CumulativeBalance
);

