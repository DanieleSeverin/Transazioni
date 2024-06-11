using Transazioni.Domain.Users;

namespace Transazioni.Domain.Account;

public interface IAccountRepository
{
    public abstract Task<Accounts?> GetById(UserId UserId, AccountId AccountId, CancellationToken cancellationToken = default);
    public abstract Task<Accounts?> GetByName(UserId UserId, AccountName AccountName, CancellationToken cancellationToken = default);
    public abstract Task<List<Accounts>> GetAccounts(UserId UserId, CancellationToken cancellationToken = default);
    public abstract void Add(Accounts Account);
    public abstract void AddRange(List<Accounts> Accounts);
}
