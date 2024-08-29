using System.ComponentModel.DataAnnotations;

namespace BenBristow.Extensions.Attributes;

/// <summary>
///     Validation attribute to ensure that a date property is before another specified date property.
///     Supports DateTime, DateTimeOffset, and DateOnly types, including their nullable versions.
/// </summary>
public sealed class MustBeBeforeAttribute : ValidationAttribute
{
    private readonly string _comparisonPropertyName;

    /// <summary>
    ///     Initializes a new instance of the <see cref="MustBeBeforeAttribute" /> class.
    /// </summary>
    /// <param name="comparisonPropertyName">The name of the property to compare against.</param>
    public MustBeBeforeAttribute(string comparisonPropertyName)
    {
        _comparisonPropertyName = comparisonPropertyName;
    }

    /// <summary>
    ///     Validates that the value of the current property is before the value of the comparison property.
    /// </summary>
    /// <param name="value">The value of the property to be validated.</param>
    /// <param name="validationContext">The context information about the validation operation.</param>
    /// <returns>
    ///     An instance of the <see cref="ValidationResult" /> class.
    ///     It will be <see cref="ValidationResult.Success" /> if the validation succeeds;
    ///     otherwise, an instance of <see cref="ValidationResult" /> with an error message.
    /// </returns>
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var property = validationContext.ObjectType.GetProperty(_comparisonPropertyName);

        if (property == null)
            return new ValidationResult($"Unknown property: {_comparisonPropertyName}");

        var comparisonValue = property.GetValue(validationContext.ObjectInstance);

        if (value == null && comparisonValue == null)
            return ValidationResult.Success;

        if (value == null || comparisonValue == null)
            return new ValidationResult(ErrorMessage ?? $"{validationContext.DisplayName} must be before {_comparisonPropertyName}.");

        var valueDate = GetDateTimeOffset(value);
        var comparisonDate = GetDateTimeOffset(comparisonValue);

        return valueDate < comparisonDate
            ? ValidationResult.Success
            : new ValidationResult(ErrorMessage ?? $"{validationContext.DisplayName} must be before {_comparisonPropertyName}.");
    }

    /// <summary>
    ///     Converts the given value to a DateTimeOffset.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    /// <returns>A DateTimeOffset representation of the value.</returns>
    /// <exception cref="ArgumentException">Thrown when the value is not a supported date type.</exception>
    private static DateTimeOffset GetDateTimeOffset(object? value) => value switch
    {
        DateTime dateTime => new DateTimeOffset(dateTime),
        DateTimeOffset dateTimeOffset => dateTimeOffset,
        DateOnly dateOnly => new DateTimeOffset(dateOnly.ToDateTime(TimeOnly.MinValue)),
        _ => throw new ArgumentException($"Unsupported date type: {value!.GetType()}"),
    };
}