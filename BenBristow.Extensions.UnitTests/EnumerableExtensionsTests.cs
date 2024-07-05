namespace BenBristow.Extensions.UnitTests;

public sealed class EnumerableExtensionsTests
{
    [Fact]
    public void WhereIf_CalledWithTrueCondition_ReturnsFilteredCollection()
    {
        // Arrange
        var source = new[] { 1, 2, 3, 4, 5 };
        const bool condition = true;

        // Act
        var result = source.WhereIf(condition, Predicate);

        // Assert
        Assert.Equal([2, 4], result);
    }

    [Fact]
    public void WhereIf_CalledWithFalseCondition_ReturnsOriginalCollection()
    {
        // Arrange
        var source = new[] { 1, 2, 3, 4, 5 };
        const bool condition = false;

        // Act
        var result = source.WhereIf(condition, Predicate);

        // Assert
        Assert.Equal(source, result);
    }

    private static bool Predicate(int x) => x % 2 == 0;
}