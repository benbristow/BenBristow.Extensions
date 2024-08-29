using System.ComponentModel.DataAnnotations;
using BenBristow.Extensions.Attributes;

namespace BenBristow.Extensions.UnitTests.Attributes;

public sealed class MustBeBeforeAttributeTests
{
    [Fact]
    public void IsValid_WhenStartDateIsBeforeEndDate_ShouldReturnSuccess()
    {
        var model = new TestModel
        {
            StartDate = new DateTime(2023, 1, 1),
            EndDate = new DateTime(2023, 1, 2),
        };
        var attribute = new MustBeBeforeAttribute(nameof(TestModel.EndDate));
        var result = attribute.GetValidationResult(model.StartDate, new ValidationContext(model) { MemberName = nameof(TestModel.StartDate) });

        result.Should().Be(ValidationResult.Success);
    }

    [Fact]
    public void IsValid_WhenStartDateIsAfterEndDate_ShouldReturnValidationError()
    {
        var model = new TestModel
        {
            StartDate = new DateTime(2023, 1, 2),
            EndDate = new DateTime(2023, 1, 1),
        };
        var attribute = new MustBeBeforeAttribute(nameof(TestModel.EndDate));
        var result = attribute.GetValidationResult(model.StartDate, new ValidationContext(model) { MemberName = nameof(TestModel.StartDate) });

        result.Should().NotBeNull();
        result!.ErrorMessage.Should().Be("StartDate must be before EndDate.");
    }

    [Fact]
    public void IsValid_WhenStartDateEqualsEndDate_ShouldReturnValidationError()
    {
        var model = new TestModel
        {
            StartDate = new DateTime(2023, 1, 1),
            EndDate = new DateTime(2023, 1, 1),
        };
        var attribute = new MustBeBeforeAttribute(nameof(TestModel.EndDate));
        var result = attribute.GetValidationResult(model.StartDate, new ValidationContext(model) { MemberName = nameof(TestModel.StartDate) });

        result.Should().NotBeNull();
        result!.ErrorMessage.Should().Be("StartDate must be before EndDate.");
    }

    [Fact]
    public void IsValid_WithCustomErrorMessage_ShouldReturnCustomMessage()
    {
        var model = new TestModel
        {
            StartDate = new DateTime(2023, 1, 2),
            EndDate = new DateTime(2023, 1, 1),
        };
        var attribute = new MustBeBeforeAttribute(nameof(TestModel.EndDate)) { ErrorMessage = "Custom error message" };
        var result = attribute.GetValidationResult(model.StartDate, new ValidationContext(model) { MemberName = nameof(TestModel.StartDate) });

        result.Should().NotBeNull();
        result!.ErrorMessage.Should().Be("Custom error message");
    }

    [Fact]
    public void IsValid_WithNullableProperties_BothNull_ShouldReturnSuccess()
    {
        var model = new TestModelWithNullables();
        var attribute = new MustBeBeforeAttribute(nameof(TestModelWithNullables.EndDate));
        var result = attribute.GetValidationResult(model.StartDate, new ValidationContext(model) { MemberName = nameof(TestModelWithNullables.StartDate) });

        result.Should().Be(ValidationResult.Success);
    }

    [Fact]
    public void IsValid_WithNullableProperties_StartDateNull_ShouldReturnValidationError()
    {
        var model = new TestModelWithNullables { EndDate = new DateTime(2023, 1, 1) };
        var attribute = new MustBeBeforeAttribute(nameof(TestModelWithNullables.EndDate));
        var result = attribute.GetValidationResult(model.StartDate, new ValidationContext(model) { MemberName = nameof(TestModelWithNullables.StartDate) });

        result.Should().NotBeNull();
        result!.ErrorMessage.Should().Be("StartDate must be before EndDate.");
    }

    [Fact]
    public void IsValid_WithDateTimeOffset_ShouldValidateCorrectly()
    {
        var model = new TestModelWithDateTimeOffset
        {
            StartDate = new DateTimeOffset(2023, 1, 1, 0, 0, 0, TimeSpan.Zero),
            EndDate = new DateTimeOffset(2023, 1, 2, 0, 0, 0, TimeSpan.Zero),
        };
        var attribute = new MustBeBeforeAttribute(nameof(TestModelWithDateTimeOffset.EndDate));
        var result = attribute.GetValidationResult(model.StartDate, new ValidationContext(model) { MemberName = nameof(TestModelWithDateTimeOffset.StartDate) });

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
        var attribute = new MustBeBeforeAttribute(nameof(TestModelWithDateOnly.EndDate));
        var result = attribute.GetValidationResult(model.StartDate, new ValidationContext(model) { MemberName = nameof(TestModelWithDateOnly.StartDate) });

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
        var attribute = new MustBeBeforeAttribute("InvalidPropertyName");
        var result = attribute.GetValidationResult(model.StartDate, new ValidationContext(model) { MemberName = nameof(TestModel.StartDate) });

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
        var attribute = new MustBeBeforeAttribute(nameof(TestModelWithUnsupportedType.EndDate));

        var action = () => attribute.GetValidationResult(model.StartDate, new ValidationContext(model) { MemberName = nameof(TestModelWithUnsupportedType.StartDate) });

        action.Should().Throw<ArgumentException>()
            .WithMessage("Unsupported date type: System.String");
    }

    private class TestModel
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    private class TestModelWithNullables
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }

    private class TestModelWithDateTimeOffset
    {
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset EndDate { get; set; }
    }

    private class TestModelWithDateOnly
    {
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
    }

    private class TestModelWithUnsupportedType
    {
        public string StartDate { get; set; } = string.Empty;
        public string EndDate { get; set; } = string.Empty;
    }
}