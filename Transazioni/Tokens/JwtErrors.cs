using Transazioni.Domain.Abstractions;

namespace Transazioni.Domain.Tokens;

public static class JwtErrors
{
    public static readonly Error NotFound = new(
        "Jwt.Found",
        "Jwt not found.");

    public static readonly Error MissingJwt = new(
        "Jwt.Missing",
        "Jwt is required.");

    public static readonly Error Invalid = new(
        "Jwt.Invalid",
        "Jwt is invalid.");

    public static readonly Error Expired = new(
        "Jwt.Expired",
        "Jwt is expired.");

    public static readonly Error IsNotJwt = new(
        "Jwt.IsNotJwt",
        "The provided string is not a valid Jwt.");

    public static readonly Error UserIdClaimNotFound = new(
        "Jwt.UserIdClaimNotFound",
        "User ID claim not found in JWT token.");
}
