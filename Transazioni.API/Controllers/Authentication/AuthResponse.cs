namespace Transazioni.API.Controllers.Authentication;

public record AuthResponse(DateTime AccessTokenExpireAt, DateTime RefreshTokenExpireAt);
