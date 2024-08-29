using System.ComponentModel.DataAnnotations;
using BenBristow.Extensions.Attributes;

namespace BenBristow.Extensions.UnitTests.Attributes;

public sealed class MustBeAfterAttributeTests
{
    [Fact]
    public void IsValid_WhenEndDateIsAfterStartDate_ShouldReturnSuccess()
    {
        var model = new TestModel
        {
            StartDate = new DateTime(2023, 1, 1),
            EndDate = new DateTime(2023, 1, 2),
        };
        var attribute = new MustBeAfterAttribute(nameof(TestModel.StartDate));
        var result = attribute.GetValidationResult(model.EndDate, new ValidationContext(model) { MemberName = nameof(TestModel.EndDate) });

        result.Should().Be(ValidationResult.Success);
    }

    [Fact]
    public void IsValid_WhenEndDateIsBeforeStartDate_ShouldReturnValidationError()
    {
        var model = new TestModel
        {
            StartDate = new DateTime(2023, 1, 2),
            EndDate = new DateTime(2023, 1, 1),
        };
        var attribute = new MustBeAfterAttribute(nameof(TestModel.StartDate));
        var result = attribute.GetValidationResult(model.EndDate, new ValidationContext(model) { MemberName = nameof(TestModel.EndDate) });

        result.Should().NotBeNull();
        result!.ErrorMessage.Should().Be("EndDate must be after StartDate.");
    }

    [Fact]
    public void IsValid_WhenEndDateEqualsStartDate_ShouldReturnValidationError()
    {
        var model = new TestModel
        {
            StartDate = new DateTime(2023, 1, 1),
            EndDate = new DateTime(2023, 1, 1),
        };
        var attribute = new MustBeAfterAttribute(nameof(TestModel.StartDate));
        var result = attribute.GetValidationResult(model.EndDate, new ValidationContext(model) { MemberName = nameof(TestModel.EndDate) });

        result.Should().NotBeNull();
        result!.ErrorMessage.Should().Be("EndDate must be after StartDate.");
    }

    [Fact]
    public void IsValid_WithCustomErrorMessage_ShouldReturnCustomMessage()
    {
        var model = new TestModel
        {
            StartDate = new DateTime(2023, 1, 2),
            EndDate = new DateTime(2023, 1, 1),
        };
        var attribute = new MustBeAfterAttribute(nameof(TestModel.StartDate)) { ErrorMessage = "Custom error message" };
        var result = attribute.GetValidationResult(model.EndDate, new ValidationContext(model) { MemberName = nameof(TestModel.EndDate) });

        result.Should().NotBeNull();
        result!.ErrorMessage.Should().Be("Custom error message");
    }

    [Fact]
    public void IsValid_WithNullableProperties_BothNull_ShouldReturnSuccess()
    {
        var model = new TestModelWithNullables();
        var attribute = new MustBeAfterAttribute(nameof(TestModelWithNullables.StartDate));
        var result = attribute.GetValidationResult(model.EndDate, new ValidationContext(model) { MemberName = nameof(TestModelWithNullables.EndDate) });

        result.Should().Be(ValidationResult.Success);
    }

    [Fact]
    public void IsValid_WithNullableProperties_EndDateNull_ShouldReturnValidationError()
    {
        var model = new TestModelWithNullables { StartDate = new DateTime(2023, 1, 1) };
        var attribute = new MustBeAfterAttribute(nameof(TestModelWithNullables.StartDate));
        var result = attribute.GetValidationResult(model.EndDate, new ValidationContext(model) { MemberName = nameof(TestModelWithNullables.EndDate) });

        result.Should().NotBeNull();
        result!.ErrorMessage.Should().Be("EndDate must be after StartDate.");
    }

    [Fact]
    public void IsValid_WithDateTimeOffset_ShouldValidateCorrectly()
    {
        var model = new TestModelWithDateTimeOffset
        {
            StartDate = new DateTimeOffset(2023, 1, 1, 0, 0, 0, TimeSpan.Zero),
            EndDate = new DateTimeOffset(2023, 1, 2, 0, 0, 0, TimeSpan.Zero),
        };
        var attribute = new MustBeAfterAttribute(nameof(TestModelWithDateTimeOffset.StartDate));
        var result = attribute.GetValidationResult(model.EndDate, new ValidationContext(model) { MemberName = nameof(TestModelWithDateTimeOffset.EndDate) });

        result.Should().Be(ValidationResult.Success);
    }

    [Fact]
    public void IsValid_WithDateOnly_ShouldValidateCorrectly()
    {
        var model = new TestModelWithDateOnly
        {
            StartDate = new DateOnly(2023, 1, 1),
            EndDate = new DateOnly(2023, 1, 2),
        };
        var attribute = new MustBeAfterAttribute(nameof(TestModelWithDateOnly.StartDate));
        var result = attribute.GetValidationResult(model.EndDate, new ValidationContext(model) { MemberName = nameof(TestModelWithDateOnly.EndDate) });

        result.Should().Be(ValidationResult.Success);
    }

    [Fact]
    public void IsValid_WithInvalidPropertyName_ShouldReturnValidationError()
    {
        var model = new TestModel
        {
            StartDate = new DateTime(2023, 1, 1),
            EndDate = new DateTime(2023, 1, 2),
        };
        var attribute = new MustBeAfterAttribute("InvalidPropertyName");
        var result = attribute.GetValidationResult(model.EndDate, new ValidationContext(model) { MemberName = nameof(TestModel.EndDate) });

        result.Should().NotBeNull();
        result!.ErrorMessage.Should().Be("Unknown property: InvalidPropertyName");
    }

    [Fact]
    public void IsValid_WithUnsupportedType_ShouldThrowArgumentException()
    {
        var model = new TestModelWithUnsupportedType
        {
            StartDate = "2023-01-01",
            EndDate = "2023-01-02",
        };
        var attribute = new MustBeAfterAttribute(nameof(TestModelWithUnsupportedType.StartDate));

        var action = () => attribute.GetValidationResult(model.EndDate, new ValidationContext(model) { MemberName = nameof(TestModelWithUnsupportedType.EndDate) });

        action.Should().Throw<ArgumentException>()
            .WithMessage("Unsupported date type: System.String");
    }

    private class TestModel
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; init; }
    }

    private class TestModelWithNullables
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }

    private class TestModelWithDateTimeOffset
    {
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset EndDate { get; init; }
    }

    private class TestModelWithDateOnly
    {
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; init; }
    }

    private class TestModelWithUnsupportedType
    {
        public string StartDate { get; set; } = string.Empty;
        public string EndDate { get; init; } = string.Empty;
    }
}