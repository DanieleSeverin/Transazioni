using Transazioni.Domain.Abstractions;

namespace Transazioni.Domain.Account;

public static class AccountErrors
{
    public static Error AlreadyExists = new(
        "Account.AlreadyExists",
        "The account already exists.");
}
