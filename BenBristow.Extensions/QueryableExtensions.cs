using System.Linq.Expressions;

namespace BenBristow.Extensions;

public static class QueryableExtensions
{
    /// <summary>
    /// Applies a predicate to filter a sequence of values based on a specified condition.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">The sequence to apply the predicate to.</param>
    /// <param name="condition">A boolean value that determines whether the predicate should be applied.</param>
    /// <param name="predicate">A function to test each element for a condition. Only invoked if <paramref name="condition"/> is true.</param>
    /// <returns>An <see cref="IQueryable{T}"/> that contains elements from the input sequence that satisfy the condition if <paramref name="condition"/> is true; otherwise, returns the original sequence.</returns>
    public static IQueryable<T> WhereIf<T>(this IQueryable<T> source, bool condition, Expression<Func<T, bool>> predicate) =>
        condition ? source.Where(predicate) : source;
}