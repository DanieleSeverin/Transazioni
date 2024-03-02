using Transazioni.Domain.Abstractions;
using Transazioni.Domain.Tokens;
using Transazioni.Domain.Users;

namespace Transazioni.Application.Abstractions.Authentication;

public interface IJwtProvider
{
    Result<AccessToken> GenerateAccessToken(User user);
    Result<RefreshToken> GenerateRefreshToken(User user);
}
