using System.Collections.Concurrent;

namespace BenBristow.Extensions.UnitTests;

public sealed class ConcurrentBagExtensionsTests
{
    [Fact]
    public void AddRange_CalledOnConcurrentBag_AddsElementsToBag()
    {
        // Arrange
        var concurrentBag = new ConcurrentBag<int>(new[] { 6, 7, 8, 9, 10 });

        // Act
        concurrentBag.AddRange(new[] { 1, 2, 3, 4, 5 });

        // Assert
        var expectedElements = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        concurrentBag.Should().BeEquivalentTo(expectedElements, options => options.WithoutStrictOrdering());
    }
}