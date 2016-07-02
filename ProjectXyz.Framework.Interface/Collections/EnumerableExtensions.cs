using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectXyz.Framework.Interface.Collections
{
    public static class EnumerableExtensions
    {
        public static void Foreach<T>(
            this IEnumerable<T> enumerable, 
            Action<T> perItemCallback)
        {
            foreach (var item in enumerable)
            {
                perItemCallback(item);
            }
        }

        public static IEnumerable<T2> TakeTypes<T2>(this IEnumerable<object> enumerable)
        {
            return enumerable.Where(x => x is T2).Cast<T2>();
        }

        public static Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> enumerable)
        {
            return enumerable.ToDictionary(
                x => x.Key,
                x => x.Value);
        }

        public static IEnumerable<T> Yield<T>(this T obj)
        {
            yield return obj;
        }

        public static T[] AsArray<T>(this T obj)
        {
            return obj.Yield().ToArray();
        }

        public static IEnumerable<T> Repeat<T>(this T obj, int times)
        {
            for (var i = 0; i < times; ++i)
            {
                yield return obj;
            }
        }

        public static IEnumerable<T> Append<T>(this IEnumerable<T> enumerable, T obj)
        {
            return enumerable.Concat(obj.Yield());
        }
    }
}