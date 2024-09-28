using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Transazioni.Application.Abstractions.Authentication;
using Transazioni.Domain.Abstractions;
using Transazioni.Domain.Tokens;
using Transazioni.Domain.Users;

namespace Transazioni.Infrastructure.Authentication.Jwt;

internal sealed class JwtProvider : IJwtProvider
{
    private readonly JwtOptions _options;

    public JwtProvider(IOptions<JwtOptions> options)
    {
        _options = options.Value;
    }

    public Result<AccessToken> GenerateAccessToken(User user)
    {
        var claims = new Claim[]
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.Value.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email.Value),
        };

        var signingCredentials = new SigningCredentials(
            new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_options.SecretKey)),
            SecurityAlgorithms.HmacSha256);

        DateTime expireAt = DateTime.UtcNow.AddHours(1);

        var token = new JwtSecurityToken(
            _options.Issuer,
            _options.Audience,
            claims,
            null,
            expireAt,
            signingCredentials);

        string tokenValue = new JwtSecurityTokenHandler().WriteToken(token);

        return Result.Success(new AccessToken(tokenValue, expireAt));
    }

    public Result<RefreshToken> GenerateRefreshToken(User user)
    {
        var randomNumber = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        string refreshToken = Convert.ToBase64String(randomNumber);
        return Result.Success(new RefreshToken(refreshToken, user.Id));
    }

    public Result<Guid> GetUserIdFromJwt(string accessToken)
    {
        JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();

        if (handler.ReadToken(accessToken) is not JwtSecurityToken jsonToken)
        {
            return Result.Failure<Guid>(JwtErrors.IsNotJwt);
        }

        var userIdClaim = jsonToken.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier || claim.Type == "sub");

        if (userIdClaim == null)
        {
            return Result.Failure<Guid>(JwtErrors.UserIdClaimNotFound);
        }

        if (Guid.TryParse(userIdClaim.Value, out var userId))
        {
            return Result.Success(userId);
        }
        else
        {
            return Result.Failure<Guid>(UserErrors.InvalidUserId);
        }
    }
}
