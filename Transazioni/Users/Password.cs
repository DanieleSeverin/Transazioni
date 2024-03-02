
using Transazioni.Domain.Abstractions;

namespace Transazioni.Domain.Users;

public record Password(string Value)
{
    public static Result<string> Validate(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
        {
            return Result.Failure<string>(PasswordErrors.Required);
        }

        if (password.Length < PasswordErrors.MinimumLength)
        {
            return Result.Failure<string>(PasswordErrors.InvalidLength);
        }

        // Check for lowercase characters
        if (!password.Any(char.IsLower))
        {
            return Result.Failure<string>(PasswordErrors.LowercaseRequired);
        }

        // Check for uppercase characters
        if (!password.Any(char.IsUpper))
        {
            return Result.Failure<string>(PasswordErrors.UppercaseRequired);
        }

        // Check for numbers
        if (!password.Any(char.IsDigit))
        {
            return Result.Failure<string>(PasswordErrors.NumberRequired);
        }

        // Check for special characters
        if (!password.Any(ch => !char.IsLetterOrDigit(ch)))
        {
            return Result.Failure<string>(PasswordErrors.SpecialCharacterRequired);
        }

        return Result.Success(password);
    }
}
