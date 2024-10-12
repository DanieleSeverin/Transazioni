using Transazioni.Application.Abstractions.Messaging;
using Transazioni.Application.Reporting.AccountsBalance.GetMonthlyCumulativeBalance;
using Transazioni.Domain.Abstractions;

namespace Transazioni.Application.Reporting.AccountsBalance.GetAccountsBalance;

public class GetMonthlyCumulativeBalanceQueryHandler : IQueryHandler<GetMonthlyCumulativeBalanceQuery, List<MonthlyAccountBalanceSummary>>
{
    private readonly IAccountBalanceProvider _accountBalanceProvider;

    public GetMonthlyCumulativeBalanceQueryHandler(IAccountBalanceProvider accountBalanceProvider)
    {
        _accountBalanceProvider = accountBalanceProvider;
    }

    public async Task<Result<List<MonthlyAccountBalanceSummary>>> Handle(GetMonthlyCumulativeBalanceQuery request, CancellationToken cancellationToken)
    {
        return await _accountBalanceProvider.GetMonthlyCumulativeBalance(cancellationToken);
    }
}
