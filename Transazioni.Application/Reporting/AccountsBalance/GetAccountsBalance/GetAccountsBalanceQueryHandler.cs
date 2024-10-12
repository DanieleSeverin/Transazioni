using Transazioni.Application.Abstractions.Messaging;
using Transazioni.Domain.Abstractions;

namespace Transazioni.Application.Reporting.AccountsBalance.GetAccountsBalance;

public class GetAccountsBalanceQueryHandler : IQueryHandler<GetAccountsBalanceQuery, List<AccountsBalanceSummary>>
{
    private readonly IAccountBalanceProvider _accountBalanceProvider;

    public GetAccountsBalanceQueryHandler(IAccountBalanceProvider accountBalanceProvider)
    {
        _accountBalanceProvider = accountBalanceProvider;
    }

    public async Task<Result<List<AccountsBalanceSummary>>> Handle(GetAccountsBalanceQuery request, CancellationToken cancellationToken)
    {
        return await _accountBalanceProvider.GetAccountsBalance(cancellationToken);
    }
}
