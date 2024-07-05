namespace BenBristow.Extensions.UnitTests;

public sealed class QueryableExtensionsTests
{
    [Fact]
    public void WhereIf_ConditionTrue_AppliesPredicate()
    {
        var source = new[] { 1, 2, 3, 4 }.AsQueryable();
        var result = source.WhereIf(true, x => x > 2);
        result.Should().BeEquivalentTo(new[] { 3, 4 });
    }

    [Fact]
    public void WhereIf_ConditionFalse_DoesNotApplyPredicate()
    {
        var source = new[] { 1, 2, 3, 4 }.AsQueryable();
        var result = source.WhereIf(false, x => x > 2);
        result.Should().BeEquivalentTo(new[] { 1, 2, 3, 4 });
    }

    [Fact]
    public void ApplyIf_ConditionTrue_AppliesTransformation()
    {
        var source = new[] { 1, 2, 3, 4 }.AsQueryable();
        var result = source.ApplyIf(true, x => x.Where(y => y > 2));
        result.Should().BeEquivalentTo(new[] { 3, 4 });
    }

    [Fact]
    public void ApplyIf_ConditionFalse_DoesNotApplyTransformation()
    {
        var source = new[] { 1, 2, 3, 4 }.AsQueryable();
        var result = source.ApplyIf(false, x => x.Where(y => y > 2));
        result.Should().BeEquivalentTo(new[] { 1, 2, 3, 4 });
    }
}