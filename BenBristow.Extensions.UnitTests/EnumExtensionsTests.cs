using System.ComponentModel;

namespace BenBristow.Extensions.UnitTests;

public sealed class EnumExtensionsTests
{
    [Fact]
    public void GetDescription_ReturnsDescription_WhenEnumValueHasDescription()
    {
        // Arrange
        const TestEnum enumValue = TestEnum.TestValueWithDescription;

        // Act
        var description = enumValue.GetDescription();

        // Assert
        description.Should().Be("Test Description");
    }

    [Fact]
    public void GetDescription_ReturnsEnumValueName_WhenEnumValueHasNoDescription()
    {
        // Arrange
        const TestEnum enumValue = TestEnum.TestValueWithoutDescription;

        // Act
        var description = enumValue.GetDescription();

        // Assert
        description.Should().Be("TestValueWithoutDescription");
    }

    [Fact]
    public void GetDescription_CalledWithInvalidEnumType_ThrowsArgumentException()
    {
        // Arrange
        const TestEnum invalidEnumValue = (TestEnum) 100;

        // Act
        Action act = () => invalidEnumValue.GetDescription();

        // Assert
        act.Should().Throw<ArgumentOutOfRangeException>()
            .WithMessage("Invalid enum value*");
    }
}

public enum TestEnum
{
    [Description("Test Description")]
    TestValueWithDescription,

    TestValueWithoutDescription
}