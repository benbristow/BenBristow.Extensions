using System.Globalization;

namespace BenBristow.Extensions.UnitTests;

public sealed class DateTimeOffsetExtensionsTests : IDisposable
{
    private readonly CultureInfo _originalCulture = Thread.CurrentThread.CurrentCulture;
    private readonly CultureInfo _originalUiCulture = Thread.CurrentThread.CurrentUICulture;

    [Theory]
    [InlineData("en-US")]
    [InlineData("de-DE")]
    [InlineData("fr-FR")]
    [InlineData("ja-JP")]
    public void ToLocaleDateTimeString_WithDifferentCultures_ShouldMatchGeneralShortPattern(string cultureName)
    {
        // Arrange
        SetCulture(cultureName);
        var dateTime = new DateTimeOffset(2023, 4, 15, 14, 30, 0, TimeSpan.Zero);

        // Act
        var result = dateTime.ToLocaleDateTimeString();

        // Assert
        var expected = dateTime.ToString("g", CultureInfo.CurrentCulture);
        result.Should().Be(expected, because: $"it should match the 'g' format for culture {cultureName}");
    }

    [Fact]
    public void ToLocaleDateTimeString_WithDifferentTimeZone_ShouldUseLocalTime()
    {
        // Arrange
        var utcDateTime = new DateTimeOffset(2023, 4, 15, 14, 30, 0, TimeSpan.Zero);
        var localDateTime = utcDateTime.ToLocalTime();

        // Act
        var result = localDateTime.ToLocaleDateTimeString();

        // Assert
        var expected = localDateTime.ToString("g", CultureInfo.CurrentCulture);
        result.Should().Be(expected, because: "it should use the local time");
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

    public void Dispose()
    {
        Thread.CurrentThread.CurrentCulture = _originalCulture;
        Thread.CurrentThread.CurrentUICulture = _originalUiCulture;
    }

    private static void SetCulture(string cultureName)
    {
        var culture = new CultureInfo(cultureName);
        Thread.CurrentThread.CurrentCulture = culture;
        Thread.CurrentThread.CurrentUICulture = culture;
    }
}