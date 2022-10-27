using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Extensions.MethodExtensions
{
    public static class EnumerableExtension
    {
        /// <summary>
        /// 连接 <typeparamref name="T"/> 数组集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        public static IEnumerable<T> ConcatEnumerables<T>(this IEnumerable<T[]> source)
        {
            var position = 0;
            var outputArray = new T[source.Sum(a => a.Length)];
            foreach (var curr in source)
            {
                Array.Copy(curr, 0, outputArray, position, curr.Length);
                position += curr.Length;
            }
            return outputArray;
        }

        /// <summary>
        /// 连接 <typeparamref name="T"/> 交错数组
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static T[] ConcatArrays<T>(this T[][] source)
        {
            return source.AsEnumerable().ConcatEnumerables().ToArray();
        }
    }
}
