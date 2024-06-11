using Transazioni.Application.Abstractions.Messaging;
using Transazioni.Domain.Abstractions;
using Transazioni.Domain.Account;
using Transazioni.Domain.Users;

namespace Transazioni.Application.Account.GetAccounts;

public class GetAccountsQueryHandler : IQueryHandler<GetAccountsQuery, List<Accounts>>
{
    private readonly IAccountRepository _accountRepository;

    public GetAccountsQueryHandler(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    public async Task<Result<List<Accounts>>> Handle(GetAccountsQuery request, CancellationToken cancellationToken)
    {
        UserId userId = new(request.UserId);
        return await _accountRepository.GetAccounts(userId, cancellationToken);
    }
}
