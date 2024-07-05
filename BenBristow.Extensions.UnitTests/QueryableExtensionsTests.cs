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
}