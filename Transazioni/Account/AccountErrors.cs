using Transazioni.Domain.Abstractions;

namespace Transazioni.Domain.Account;

public static class AccountErrors
{
    public static readonly Error AlreadyExists = new(
        "Account.AlreadyExists",
        "The account already exists.");

    public static readonly Error NotFound = new(
        "Account.NotFound",
        "Account not found.");
}
