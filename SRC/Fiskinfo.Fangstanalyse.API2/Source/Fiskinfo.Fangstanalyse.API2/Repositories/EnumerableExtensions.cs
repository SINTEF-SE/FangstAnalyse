using System;
using System.Collections.Generic;

namespace Fiskinfo.Fangstanalyse.API2.Repositories
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> If<T>(this IEnumerable<T> enumerable, bool condition, Func<IEnumerable<T>, IEnumerable<T>> action)
        {
            return condition ? action(enumerable) : enumerable;
        }
    }
}