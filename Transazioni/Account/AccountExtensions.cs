namespace Transazioni.Domain.Account;

public static class AccountExtensions
{
    public static Accounts? FindByName(this List<Accounts> List, AccountName AccountName)
    {
        string accountName = AccountName.Value.Trim().ToLower();
        return List.Find(acc => acc.AccountName.Value.Trim().ToLower() == accountName);
    }

    public static Accounts? FindByName(this List<Accounts> List, string AccountName)
    {
        string accountName = AccountName.Trim().ToLower();
        return List.Find(acc => acc.AccountName.Value.Trim().ToLower() == accountName);
    }
}
