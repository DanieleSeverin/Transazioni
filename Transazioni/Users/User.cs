using Transazioni.Domain.Account;
using Transazioni.Domain.AccountRule;
using Transazioni.Domain.Movement;
using Transazioni.Domain.Tokens;

namespace Transazioni.Domain.Users;

public class User
{
    public UserId Id { get; init; }
    public FirstName FirstName { get; private set; }

    public LastName LastName { get; private set; }

    public Email Email { get; private set; }

    public Password Password { get; private set; }

    #pragma warning disable S2365
    private readonly List<RefreshToken> _refreshTokens = new();
    public IReadOnlyList<RefreshToken> RefreshTokens => _refreshTokens.ToList();

    private readonly List<Accounts> _accounts = new();
    public IReadOnlyList<Accounts> Accounts => _accounts.ToList();

    private readonly List<AccountRules> _accountRules = new();
    public IReadOnlyList<AccountRules> AccountRules => _accountRules.ToList();
    private readonly List<Movements> _movements = new();
    public IReadOnlyList<Movements> Movements => _movements.ToList();
    #pragma warning disable S2365

    private User(UserId id,
                 FirstName firstName,
                 LastName lastName,
                 Email email,
                 Password password)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Password = password;
    }

    #pragma warning disable CS8618
    private User()
    {
    }
    #pragma warning restore CS8618

    public static User Create(FirstName firstName, LastName lastName, Email email, Password password)
    {
        var user = new User(UserId.New(), firstName, lastName, email, password);

        return user;
    }

    public void AddRefreshToken(RefreshToken refreshToken)
    {
        _refreshTokens.Where(t => t.Valid).ToList().ForEach(token => token.Invalidate("Added new Refresh Token."));
        _refreshTokens.Add(refreshToken);
    }
}