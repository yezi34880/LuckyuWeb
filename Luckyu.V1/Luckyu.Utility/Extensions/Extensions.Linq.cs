using System;
using System.Collections.Generic;


namespace Luckyu.Utility
{
    /// <summary>
    /// 描 述：List扩展
    /// </summary>
    public static partial class Extensions
    {
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))

                {
                    yield return element;
                }
            }
        }
    }
}
