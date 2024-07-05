using System.Security.Cryptography;
using System.Text;

namespace BenBristow.Extensions;

public static class StringExtensions
{
    /// <summary>
    /// Converts the first character of a string to uppercase.
    /// </summary>
    /// <param name="value">The string to convert.</param>
    /// <returns>A string with the first character in uppercase and the rest of the string unchanged.</returns>
    public static string UppercaseFirstLetter(this string value) => string.Concat(value[..1].ToUpper(), value.AsSpan(1));

    /// <summary>
    /// Computes the SHA-512 hash for the input string.
    /// </summary>
    /// <param name="value">The input string to hash.</param>
    /// <returns>A hexadecimal string representation of the SHA-512 hash.</returns>
    public static string HashSHA512(this string value)
    {
        var message = Encoding.UTF8.GetBytes(value);
        var hashValue = SHA512.HashData(message);
        return hashValue.Aggregate("", (current, x) => $"{current}{x:x2}");
    }
}