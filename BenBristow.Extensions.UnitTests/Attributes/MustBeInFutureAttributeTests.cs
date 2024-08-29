using System.ComponentModel.DataAnnotations;
using BenBristow.Extensions.Attributes;

namespace BenBristow.Extensions.UnitTests.Attributes;

public sealed class MustBeInFutureAttributeTests
{
    [Fact]
    public void IsValid_WithNullValue_ShouldReturnTrue()
    {
        // Arrange
        var attribute = new MustBeInFutureAttribute();

        // Act
        var result = attribute.IsValid(null);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void IsValid_WithFutureDateTime_ShouldReturnTrue()
    {
        // Arrange
        var attribute = new MustBeInFutureAttribute();
        var futureDate = DateTime.Now.AddDays(1);

        // Act
        var result = attribute.IsValid(futureDate);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void IsValid_WithPastDateTime_ShouldReturnFalse()
    {
        // Arrange
        var attribute = new MustBeInFutureAttribute();
        var pastDate = DateTime.Now.AddDays(-1);

        // Act
        var result = attribute.IsValid(pastDate);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void IsValid_WithFutureDateTimeOffset_ShouldReturnTrue()
    {
        // Arrange
        var attribute = new MustBeInFutureAttribute();
        var futureDate = DateTimeOffset.Now.AddDays(1);

        // Act
        var result = attribute.IsValid(futureDate);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void IsValid_WithPastDateTimeOffset_ShouldReturnFalse()
    {
        // Arrange
        var attribute = new MustBeInFutureAttribute();
        var pastDate = DateTimeOffset.Now.AddDays(-1);

        // Act
        var result = attribute.IsValid(pastDate);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void IsValid_WithFutureDateOnly_ShouldReturnTrue()
    {
        // Arrange
        var attribute = new MustBeInFutureAttribute();
        var futureDate = DateOnly.FromDateTime(DateTime.Now.AddDays(1));

        // Act
        var result = attribute.IsValid(futureDate);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void IsValid_WithPastDateOnly_ShouldReturnFalse()
    {
        // Arrange
        var attribute = new MustBeInFutureAttribute();
        var pastDate = DateOnly.FromDateTime(DateTime.Now.AddDays(-1));

        // Act
        var result = attribute.IsValid(pastDate);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void IsValid_WithInvalidType_ShouldThrowArgumentException()
    {
        // Arrange
        var attribute = new MustBeInFutureAttribute();

        // Act
        Action act = () => attribute.IsValid("not a date");

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("Invalid value type (Parameter 'value')");
    }

    [Fact]
    public void ValidationResult_WithFutureDate_ShouldReturnSuccess()
    {
        // Arrange
        var attribute = new MustBeInFutureAttribute();
        var futureDate = DateTime.Now.AddDays(1);
        var validationContext = new ValidationContext(new object());

        // Act
        var result = attribute.GetValidationResult(futureDate, validationContext);

        // Assert
        result.Should().Be(ValidationResult.Success);
    }

    [Fact]
    public void ValidationResult_WithPastDate_ShouldReturnValidationError()
    {
        // Arrange
        var attribute = new MustBeInFutureAttribute();
        var pastDate = DateTime.Now.AddDays(-1);
        var validationContext = new ValidationContext(new object()) { MemberName = "TestDate" };

        // Act
        var result = attribute.GetValidationResult(pastDate, validationContext);

        // Assert
        result.Should().NotBeNull();
        result!.ErrorMessage.Should().Be("The TestDate must be a date in the future.");
        result.MemberNames.Should().ContainSingle().Which.Should().Be("TestDate");
    }

    [Fact]
    public void ValidationResult_WithCustomErrorMessage_ShouldUseCustomMessage()
    {
        // Arrange
        var customMessage = "Custom error message for {0}";
        var attribute = new MustBeInFutureAttribute(customMessage);
        var pastDate = DateTime.Now.AddDays(-1);
        var validationContext = new ValidationContext(new object()) { MemberName = "TestDate" };

        // Act
        var result = attribute.GetValidationResult(pastDate, validationContext);

        // Assert
        result.Should().NotBeNull();
        result!.ErrorMessage.Should().Be("Custom error message for TestDate");
    }
}