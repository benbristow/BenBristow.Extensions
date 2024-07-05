using System.Collections.Concurrent;

namespace BenBristow.Extensions;

public static class ConcurrentBagExtensions
{
    /// <summary>
    /// Adds a range of elements to the <see cref="ConcurrentBag{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the collection.</typeparam>
    /// <param name="concurrentBag">The <see cref="ConcurrentBag{T}"/> to add items to.</param>
    /// <param name="toAdd">The elements to add to the <see cref="ConcurrentBag{T}"/>.</param>
    public static void AddRange<T>(this ConcurrentBag<T> concurrentBag, IEnumerable<T> toAdd) =>
        Parallel.ForEach(toAdd, concurrentBag.Add);
}