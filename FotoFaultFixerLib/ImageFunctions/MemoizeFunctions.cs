using System;
using System.Collections.Concurrent;

namespace FotoFaultFixerLib.ImageFunctions
{
    /// <summary>
    /// Set of special extention functions that allow us to memoize functions cutting down processing time.
    /// </summary>
    /// <remarks>
    /// Source: https://scale-tone.github.io/2017/09/18/memoization-tricks
    /// PG: Don't use these with non-primative method arguments (objects, etc...) as it wont't work and likely will introduce memory leaks!
    /// </remarks>
    public static class MemoizeFunctions
    {
        // Memoize Function with 1 param
        public static Func<T1, TResult> Memoize<T1, TResult>(this Func<T1, TResult> func)
        {
            var cache = new ConcurrentDictionary<T1, TResult>();
            return key => cache.GetOrAdd(key, func);
        }

        // Memoize Function with 2 params
        public static Func<T1, T2, TResult> Memoize<T1, T2, TResult>(this Func<T1, T2, TResult> func)
        {
            var memoizedFunc = new Func<Tuple<T1, T2>, TResult>(tuple => func(tuple.Item1, tuple.Item2)).Memoize();
            return (t1, t2) => memoizedFunc(new Tuple<T1, T2>(t1, t2));
        }

        // Memoize Function with 3 params
        public static Func<T1, T2, T3, TResult> Memoize<T1, T2, T3, TResult>(this Func<T1, T2, T3, TResult> func)
        {
            var memoizedFunc = new Func<Tuple<T1, T2, T3>, TResult>(tuple => func(tuple.Item1, tuple.Item2, tuple.Item3)).Memoize();
            return (t1, t2, t3) => memoizedFunc(new Tuple<T1, T2, T3>(t1, t2, t3));
        }
    }
}
