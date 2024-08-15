using System.Linq.Expressions;
using BenBristow.Extensions.Enums;

namespace BenBristow.Extensions.UnitTests;

public sealed class QueryableExtensionsTests
{
    #region Test Data and Setup

    private class TestClass
    {
        public int Id { get; init; }
        public string Title { get; init; } = string.Empty;
    }

    public enum TestSortField
    {
        Id,

        Title,
    }

    private static readonly List<TestClass> TestData = new()
    {
        new TestClass { Id = 3, Title = "Charlie" },
        new TestClass { Id = 1, Title = "Alpha" },
        new TestClass { Id = 2, Title = "BRAVO" },
    };

    private static readonly Dictionary<(TestSortField, SortDirection), Func<IQueryable<TestClass>, IOrderedQueryable<TestClass>>> OrderConfig = new()
    {
        { (TestSortField.Id, SortDirection.Ascending), q => q.OrderBy(t => t.Id) },
        { (TestSortField.Id, SortDirection.Descending), q => q.OrderByDescending(t => t.Id) },
        { (TestSortField.Title, SortDirection.Ascending), q => q.OrderBy(t => t.Title.ToLower()) },
        { (TestSortField.Title, SortDirection.Descending), q => q.OrderByDescending(t => t.Title.ToLower()) },
    };

    private static Expression<Func<TestClass, object>> DefaultSort(TestSortField? field) => field switch
    {
        TestSortField.Id => t => t.Id,
        TestSortField.Title => t => t.Title.ToLower(),
        null => t => t.Id,
        _ => throw new ArgumentOutOfRangeException(nameof(field), "Invalid sort field"),
    };

    #endregion

    #region WhereIf Tests

    [Fact]
    public void WhereIf_WhenConditionIsTrue_ShouldApplyPredicate()
    {
        // Arrange
        var source = new[] { 1, 2, 3, 4, 5 }.AsQueryable();

        // Act
        var result = source.WhereIf(true, x => x > 3);

        // Assert
        result.Should().BeEquivalentTo(new[] { 4, 5 });
    }

    [Fact]
    public void WhereIf_WhenConditionIsFalse_ShouldNotApplyPredicate()
    {
        // Arrange
        var source = new[] { 1, 2, 3, 4, 5 }.AsQueryable();

        // Act
        var result = source.WhereIf(false, x => x > 3);

        // Assert
        result.Should().BeEquivalentTo(new[] { 1, 2, 3, 4, 5 });
    }

    #endregion

    #region ApplyIf Tests

    [Fact]
    public void ApplyIf_WhenConditionIsTrue_ShouldApplyTransformation()
    {
        // Arrange
        var source = new[] { 1, 2, 3, 4, 5 }.AsQueryable();

        // Act
        var result = source.ApplyIf(true, q => q.Where(x => x % 2 == 0).Select(x => x * 2));

        // Assert
        result.Should().BeEquivalentTo(new[] { 4, 8 });
    }

    [Fact]
    public void ApplyIf_WhenConditionIsFalse_ShouldNotApplyTransformation()
    {
        // Arrange
        var source = new[] { 1, 2, 3, 4, 5 }.AsQueryable();

        // Act
        var result = source.ApplyIf(false, q => q.Where(x => x % 2 == 0).Select(x => x * 2));

        // Assert
        result.Should().BeEquivalentTo(new[] { 1, 2, 3, 4, 5 });
    }

    #endregion

    #region OrderBy Tests

    [Theory]
    [InlineData(TestSortField.Id, SortDirection.Ascending, new[] { 1, 2, 3 })]
    [InlineData(TestSortField.Id, SortDirection.Descending, new[] { 3, 2, 1 })]
    [InlineData(TestSortField.Title, SortDirection.Ascending, new[] { 1, 2, 3 })]
    [InlineData(TestSortField.Title, SortDirection.Descending, new[] { 3, 2, 1 })]
    public void OrderBy_WithConfiguredSortFieldAndDirection_ShouldSortCorrectly(TestSortField sortField, SortDirection sortDirection, int[] expectedOrder)
    {
        // Arrange
        var queryable = TestData.AsQueryable();

        // Act
        var result = queryable.OrderBy(sortField, sortDirection, OrderConfig, DefaultSort).ToList();

        // Assert
        result.Select(t => t.Id).Should().BeEquivalentTo(expectedOrder, options => options.WithStrictOrdering());
    }

    [Theory]
    [InlineData(TestSortField.Title, SortDirection.Ascending, new[] { "Alpha", "BRAVO", "Charlie" })]
    [InlineData(TestSortField.Title, SortDirection.Descending, new[] { "Charlie", "BRAVO", "Alpha" })]
    public void OrderBy_WhenSortingByTitle_ShouldSortCaseInsensitively(TestSortField sortField, SortDirection sortDirection, string[] expectedOrder)
    {
        // Arrange
        var queryable = TestData.AsQueryable();

        // Act
        var result = queryable.OrderBy(sortField, sortDirection, OrderConfig, DefaultSort).ToList();

        // Assert
        result.Select(t => t.Title).Should().BeEquivalentTo(expectedOrder, options => options.WithStrictOrdering());
    }

    [Fact]
    public void OrderBy_WhenSortFieldIsNull_ShouldUseDefaultSort()
    {
        // Arrange
        var queryable = TestData.AsQueryable();

        // Act
        var result = queryable.OrderBy(null, SortDirection.Ascending, OrderConfig, DefaultSort).ToList();

        // Assert
        result.Select(t => t.Id).Should().BeEquivalentTo(new[] { 1, 2, 3 }, options => options.WithStrictOrdering());
    }

    [Fact]
    public void OrderBy_WhenSortFieldIsNullAndDirectionIsDescending_ShouldUseDefaultSortDescending()
    {
        // Arrange
        var queryable = TestData.AsQueryable();

        // Act
        var result = queryable.OrderBy(null, SortDirection.Descending, OrderConfig, DefaultSort).ToList();

        // Assert
        result.Select(t => t.Id).Should().BeEquivalentTo(new[] { 3, 2, 1 }, options => options.WithStrictOrdering());
    }

    [Fact]
    public void OrderBy_WhenSortDirectionIsNull_ShouldSortAscending()
    {
        // Arrange
        var queryable = TestData.AsQueryable();

        // Act
        var result = queryable.OrderBy(TestSortField.Id, null, OrderConfig, DefaultSort).ToList();

        // Assert
        result.Select(t => t.Id).Should().BeEquivalentTo(new[] { 1, 2, 3 }, options => options.WithStrictOrdering());
    }

    [Fact]
    public void OrderBy_WhenBothSortFieldAndDirectionAreNull_ShouldUseDefaultSortAscending()
    {
        // Arrange
        var queryable = TestData.AsQueryable();

        // Act
        var result = queryable.OrderBy(null, null, OrderConfig, DefaultSort).ToList();

        // Assert
        result.Select(t => t.Id).Should().BeEquivalentTo(new[] { 1, 2, 3 }, options => options.WithStrictOrdering());
    }

    [Fact]
    public void OrderBy_WhenGivenInvalidSortDirection_ShouldThrowArgumentException()
    {
        // Arrange
        var queryable = TestData.AsQueryable();
        const SortDirection invalidSortDirection = (SortDirection)999;

        // Act & Assert
        queryable.Invoking(q => q.OrderBy(TestSortField.Id, invalidSortDirection, OrderConfig, DefaultSort).ToList())
            .Should().Throw<ArgumentException>()
            .WithMessage("Sort configuration not found for field Id and direction 999");
    }

    [Fact]
    public void OrderBy_WhenGivenUnconfiguredSortFieldAndDirection_ShouldThrowArgumentException()
    {
        // Arrange
        var queryable = TestData.AsQueryable();
        var incompleteOrderConfig = new Dictionary<(TestSortField, SortDirection), Func<IQueryable<TestClass>, IOrderedQueryable<TestClass>>>
        {
            { (TestSortField.Id, SortDirection.Ascending), q => q.OrderBy(t => t.Id) },
        };

        // Act & Assert
        queryable.Invoking(q => q.OrderBy(TestSortField.Title, SortDirection.Descending, incompleteOrderConfig, DefaultSort).ToList())
            .Should().Throw<ArgumentException>()
            .WithMessage("Sort configuration not found for field Title and direction Descending");
    }

    #endregion
}