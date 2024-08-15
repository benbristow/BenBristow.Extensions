using System.Linq.Expressions;
using BenBristow.Extensions.Enums;

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

    /// <summary>
    /// Provides a flexible way to order an IQueryable based on an enum sort field, sort direction, a configuration dictionary, and a default sort expression.
    /// </summary>
    /// <typeparam name="TEnum">The enum type used to specify the sort field.</typeparam>
    /// <typeparam name="T">The type of elements in the source IQueryable.</typeparam>
    /// <param name="source">The source IQueryable to be ordered.</param>
    /// <param name="sortField">The enum value specifying which field to sort by, or null for default sorting.</param>
    /// <param name="sortDirection">The direction to sort (Ascending or Descending). If null, defaults to Ascending.</param>
    /// <param name="orderConfig">A dictionary containing preconfigured ordering strategies for specific enum values and sort directions.</param>
    /// <param name="defaultSort">An expression defining the default sort to be applied when sortField is null.</param>
    /// <returns>An IOrderedQueryable of T, sorted according to the specified field, direction, configuration, or default sort.</returns>
    /// <remarks>
    /// If a matching configuration is found in orderConfig for the given sortField and sortDirection, it will be used.
    /// If sortField is null, the default sort will be applied using the defaultSort expression.
    /// If sortDirection is null, it will default to Ascending.
    /// The default sort respects the specified or default sort direction.
    /// </remarks>
    /// <exception cref="ArgumentException">Thrown when the sort field/direction combination is not configured in orderConfig.</exception>
    public static IOrderedQueryable<T> OrderBy<TEnum, T>(
        this IQueryable<T> source,
        TEnum? sortField,
        SortDirection? sortDirection,
        Dictionary<(TEnum, SortDirection), Func<IQueryable<T>, IOrderedQueryable<T>>> orderConfig,
        Expression<Func<T, object>> defaultSort)
        where TEnum : struct, Enum
        where T : class
    {
        if (!sortField.HasValue)
            return sortDirection == SortDirection.Descending
                ? source.OrderByDescending(defaultSort)
                : source.OrderBy(defaultSort);

        var direction = sortDirection ?? SortDirection.Ascending;
        if (orderConfig.TryGetValue((sortField.Value, direction), out var configuredOrder))
            return configuredOrder(source);

        throw new ArgumentException($"Sort configuration not found for field {sortField.Value} and direction {direction}");
    }
}