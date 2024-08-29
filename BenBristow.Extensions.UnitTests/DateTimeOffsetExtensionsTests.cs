using System.Globalization;

namespace BenBristow.Extensions.UnitTests;

public sealed class DateTimeOffsetExtensionsTests
{
    [Fact]
    public void ToLocaleDateTimeString_WithKnownDate_ShouldReturnExpectedFormat()
    {
        // Arrange
        var dateTime = new DateTimeOffset(2023, 4, 15, 14, 30, 0, TimeSpan.Zero);
        var expectedFormat = "4/15/2023 2:30 PM"; // Assuming en-US culture

        // Act
        var result = dateTime.ToLocaleDateTimeString();

        // Assert
        result.Should().Be(expectedFormat);
    }

    [Fact]
    public void ToLocaleDateTimeString_WithDifferentCulture_ShouldReturnCultureSpecificFormat()
    {
        // Arrange
        var dateTime = new DateTimeOffset(2023, 4, 15, 14, 30, 0, TimeSpan.Zero);
        var culture = new CultureInfo("de-DE");
        var expectedFormat = "15.04.2023 14:30"; // German format

        // Act
        var originalCulture = CultureInfo.CurrentCulture;
        try
        {
            CultureInfo.CurrentCulture = culture;
            var result = dateTime.ToLocaleDateTimeString();

            // Assert
            result.Should().Be(expectedFormat);
        }
        finally
        {
            CultureInfo.CurrentCulture = originalCulture;
        }
    }

    [Fact]
    public void ToLocaleDateTimeString_WithDifferentTimeZone_ShouldReturnLocalTime()
    {
        // Arrange
        var utcDateTime = new DateTimeOffset(2023, 4, 15, 14, 30, 0, TimeSpan.Zero);
        var pacificTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");
        var pacificDateTime = TimeZoneInfo.ConvertTime(utcDateTime, pacificTimeZone);

        // The expected format will depend on the exact offset, which can vary due to daylight saving time
        var expectedHour = pacificDateTime.Hour;
        var expectedAmPm = expectedHour >= 12 ? "PM" : "AM";
        if (expectedHour > 12) expectedHour -= 12;
        if (expectedHour == 0) expectedHour = 12;

        // Act
        var result = pacificDateTime.ToLocaleDateTimeString();

        // Assert
        result.Should().Contain($"{expectedHour}:");
        result.Should().EndWith(expectedAmPm);
    }

    [Fact]
    public void ToLocaleDateTimeString_WithMinDateTimeOffset_ShouldNotThrowException()
    {
        // Arrange
        var minDate = DateTimeOffset.MinValue;

        // Act
        Action act = () => minDate.ToLocaleDateTimeString();

        // Assert
        act.Should().NotThrow();
    }

    [Fact]
    public void ToLocaleDateTimeString_WithMaxDateTimeOffset_ShouldNotThrowException()
    {
        // Arrange
        var maxDate = DateTimeOffset.MaxValue;

        // Act
        Action act = () => maxDate.ToLocaleDateTimeString();

        // Assert
        act.Should().NotThrow();
    }
}