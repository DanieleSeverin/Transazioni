using Transazioni.Domain.Abstractions;
using Transazioni.Domain.Tokens;
using Transazioni.Domain.Users;

namespace Transazioni.Application.Abstractions.Authentication;

public interface IJwtProvider
{
    public abstract Result<AccessToken> GenerateAccessToken(User user);
    public abstract Result<RefreshToken> GenerateRefreshToken(User user);
    public abstract Result<Guid> GetUserIdFromJwt(string accessToken);
}
