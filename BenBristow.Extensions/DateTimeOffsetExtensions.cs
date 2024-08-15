namespace BenBristow.Extensions;

public static class DateTimeOffsetExtensions
{
    public static string ToLocaleDateTimeString(this DateTimeOffset dateTime) => dateTime.ToString("g");
}