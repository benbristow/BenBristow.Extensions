namespace BenBristow.Extensions;

/// <summary>
/// Provides extension methods for DateTimeOffset.
/// </summary>
public static class DateTimeOffsetExtensions
{
    /// <summary>
    /// Converts the specified DateTimeOffset to a locale-specific datetime string.
    /// </summary>
    /// <param name="dateTime">The DateTimeOffset to convert.</param>
    /// <returns>A locale-specific datetime string representation of the DateTimeOffset.</returns>
    public static string ToLocaleDateTimeString(this DateTimeOffset dateTime) => dateTime.ToString("g");
}