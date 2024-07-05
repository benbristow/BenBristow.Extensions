namespace BenBristow.Extensions.UnitTests;

public sealed class QueryableExtensionsTests
{
    [Fact]
    public void WhereIf_ConditionTrue_AppliesPredicate()
    {
        // Arrange
        var source = new[] { 1, 2, 3, 4 }.AsQueryable();

        // Act
        var result = source.WhereIf(true, x => x > 2);

        // Assert
        result.Should().BeEquivalentTo(new[] { 3, 4 });
    }

    [Fact]
    public void WhereIf_ConditionFalse_DoesNotApplyPredicate()
    {
        // Arrange
        var source = new[] { 1, 2, 3, 4 }.AsQueryable();

        // Act
        var result = source.WhereIf(false, x => x > 2);

        // Assert
        result.Should().BeEquivalentTo(new[] { 1, 2, 3, 4 });
    }

    [Fact]
    public void ApplyIf_ConditionTrue_AppliesTransformation()
    {
        // Arrange
        var source = new[] { 1, 2, 3, 4 }.AsQueryable();

        // Act
        var result = source.ApplyIf(true, x => x.Where(y => y > 2));

        // Assert
        result.Should().BeEquivalentTo(new[] { 3, 4 });
    }

    [Fact]
    public void ApplyIf_ConditionFalse_DoesNotApplyTransformation()
    {
        // Arrange
        var source = new[] { 1, 2, 3, 4 }.AsQueryable();

        // Act
        var result = source.ApplyIf(false, x => x.Where(y => y > 2));

        // Assert
        result.Should().BeEquivalentTo(new[] { 1, 2, 3, 4 });
    }
}