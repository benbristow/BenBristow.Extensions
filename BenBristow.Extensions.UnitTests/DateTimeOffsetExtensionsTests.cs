using System.Globalization;

namespace BenBristow.Extensions.UnitTests;

public sealed class DateTimeOffsetExtensionsTests : IDisposable
{
    private readonly CultureInfo _originalCulture = Thread.CurrentThread.CurrentCulture;

    [Fact]
    public void ToLocaleDateTimeString_WithKnownDate_ShouldReturnExpectedFormat()
    {
        // Arrange
        SetCulture("en-US");
        var dateTime = new DateTimeOffset(2023, 4, 15, 14, 30, 0, TimeSpan.Zero);
        const string expectedFormat = "4/15/2023 2:30\u202fPM";

        // Act
        var result = dateTime.ToLocaleDateTimeString();

        // Assert
        result.Should().Be(expectedFormat);
    }

    [Fact]
    public void ToLocaleDateTimeString_WithDifferentCulture_ShouldReturnCultureSpecificFormat()
    {
        // Arrange
        SetCulture("de-DE");
        var dateTime = new DateTimeOffset(2023, 4, 15, 14, 30, 0, TimeSpan.Zero);
        const string expectedFormat = "15.04.2023 14:30";

        // Act
        var result = dateTime.ToLocaleDateTimeString();

        // Assert
        result.Should().Be(expectedFormat);
    }

    [Fact]
    public void ToLocaleDateTimeString_WithDifferentTimeZone_ShouldReturnLocalTime()
    {
        // Arrange
        SetCulture("en-US");
        var utcDateTime = new DateTimeOffset(2023, 4, 15, 14, 30, 0, TimeSpan.Zero);
        var pacificTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");
        var pacificDateTime = TimeZoneInfo.ConvertTime(utcDateTime, pacificTimeZone);

        // Act
        var result = pacificDateTime.ToLocaleDateTimeString();

        // Assert
        result.Should().Contain(pacificDateTime.ToString("tt", CultureInfo.InvariantCulture));
    }

    [Fact]
    public void ToLocaleDateTimeString_WithMinDateTimeOffset_ShouldNotThrowException()
    {
        // Arrange
        SetCulture("en-US");
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
        SetCulture("en-US");
        var maxDate = DateTimeOffset.MaxValue;

        // Act
        Action act = () => maxDate.ToLocaleDateTimeString();

        // Assert
        act.Should().NotThrow();
    }

    public void Dispose() => Thread.CurrentThread.CurrentCulture = _originalCulture;

    private static void SetCulture(string cultureName)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo(cultureName);
        Thread.CurrentThread.CurrentUICulture = new CultureInfo(cultureName);
    }
}