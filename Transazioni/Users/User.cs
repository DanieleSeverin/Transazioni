using Transazioni.Domain.Tokens;

namespace Transazioni.Domain.Users;

public sealed class User
{
    public UserId Id { get; init; }
    public FirstName FirstName { get; private set; }

    public LastName LastName { get; private set; }

    public Email Email { get; private set; }

    public Password Password { get; private set; }

    private readonly List<RefreshToken> _refreshTokens = new();
    public IReadOnlyList<RefreshToken> RefreshTokens => _refreshTokens.ToList();

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

    private User()
    {
    }

    public static User Create(FirstName firstName, LastName lastName, Email email, Password password)
    {
        var user = new User(UserId.New(), firstName, lastName, email, password);

        return user;
    }

    public void AddRefreshToken(RefreshToken refreshToken)
    {
        _refreshTokens.Add(refreshToken);
    }
}