namespace Transazioni.Domain.Account;

public interface IAccountRepository
{
    public abstract Task<Accounts?> GetByName(AccountName AccountName, CancellationToken cancellationToken = default);
    public abstract Task<List<Accounts>> GetAccounts(CancellationToken cancellationToken = default);
    public abstract void Add(Accounts Account);
    public abstract void AddRange(List<Accounts> Accounts);
}
