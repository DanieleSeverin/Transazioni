using Transazioni.Domain.Abstractions;

namespace Transazioni.Domain.Users;

public static class PasswordErrors
{
    public static int MinimumLength { get; } = 8;

    public static Error Required = new(
    "Password.Required",
    "The password is required.");

    public static Error InvalidLength = new(
        "Password.InvalidLength",
        $"The minumum length for password is {MinimumLength}");

    public static Error LowercaseRequired = new(
    "Password.LowercaseRequired",
    "The password must contain at least one lowercase character.");

    public static Error UppercaseRequired = new(
        "Password.UppercaseRequired",
        "The password must contain at least one uppercase character.");

    public static Error NumberRequired = new(
        "Password.NumberRequired",
        "The password must contain at least one number.");

    public static Error SpecialCharacterRequired = new(
        "Password.SpecialCharacterRequired",
        "The password must contain at least one special character.");
}
