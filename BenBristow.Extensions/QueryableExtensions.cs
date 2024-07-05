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

    /// <summary>
    /// Conditionally applies a transformation function to a <see cref="IQueryable{T}"/> based on a provided condition.
    /// If the condition is true, the transformation function is applied to the source queryable. Otherwise, the original source queryable is returned unchanged.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the source <see cref="IQueryable{T}"/>.</typeparam>
    /// <param name="source">The source <see cref="IQueryable{T}"/> to potentially transform.</param>
    /// <param name="condition">A boolean value that determines whether the transformation function should be applied.</param>
    /// <param name="func">The transformation function to apply to the source <see cref="IQueryable{T}"/> if the condition is true.</param>
    /// <returns>An <see cref="IQueryable{T}"/> that is either the original source or the result of applying the transformation function, based on the condition.</returns>
    public static IQueryable<T> ApplyIf<T>(this IQueryable<T> source, bool condition, Func<IQueryable<T>, IQueryable<T>> func) =>
        condition ? func(source) : source;
}