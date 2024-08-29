namespace BenBristow.Extensions;

/// <summary>
/// Provides extension methods for DateTimeOffset.
/// </summary>
public static class DateTimeOffsetExtensions
{
    /// <summary>
    /// Converts a DateTimeOffset to a locale-specific date and time string.
    /// </summary>
    /// <param name="dateTime">The DateTimeOffset to convert.</param>
    /// <returns>A string representation of the date and time in the format "M/d/yyyy h:mm tt" (for en-US culture).</returns>
    /// <remarks>
    /// This method uses the "g" custom format specifier, which represents the general short date and short time pattern.
    /// The exact output may vary depending on the current culture settings.
    /// </remarks>
    public static string ToLocaleDateTimeString(this DateTimeOffset dateTime) => dateTime.ToString("g");
}