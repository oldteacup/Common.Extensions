using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Common.Extensions.MethodExtensions
{
    public static class DeepCopyExtension
    {
        public static T DeepCopy<T>(this T source)
        {
            return source.DeepCopy<T, T>();
        }

        public static void DeepCopyTo<TIn, TOut>(this TIn source, TOut destination)
        {
            destination = source.DeepCopy<TIn, TOut>();
        }

        public static TOut DeepCopy<TIn, TOut>(this TIn tIn)
        {
            ParameterExpression parameterExpression = Expression.Parameter(typeof(TIn), "p");
            List<MemberBinding> memberBindingList = new List<MemberBinding>();

            foreach (var item in typeof(TOut).GetProperties())
            {
                if (!item.CanWrite)
                {
                    continue;
                }

                MemberExpression property = Expression.Property(parameterExpression, typeof(TIn).GetProperty(item.Name));
                MemberBinding memberBinding = Expression.Bind(item, property);
                memberBindingList.Add(memberBinding);
            }

            MemberInitExpression memberInitExpression = Expression.MemberInit(Expression.New(typeof(TOut)), memberBindingList.ToArray());
            Expression<Func<TIn, TOut>> lambda = Expression.Lambda<Func<TIn, TOut>>(memberInitExpression, new ParameterExpression[] { parameterExpression });

            return lambda.Compile().Invoke(tIn);
        }
    }


    //public static class TransExpV2<TIn, TOut>
    //{
    //    private static readonly Func<TIn, TOut> cache = GetFunc();
    //    private static Func<TIn, TOut> GetFunc()
    //    {
    //        ParameterExpression parameterExpression = Expression.Parameter(typeof(TIn), "p");
    //        List<MemberBinding> memberBindingList = new List<MemberBinding>();

    //        foreach (var item in typeof(TOut).GetProperties())
    //        {
    //            if (!item.CanWrite)
    //                continue;

    //            MemberExpression property = Expression.Property(parameterExpression, typeof(TIn).GetProperty(item.Name));
    //            MemberBinding memberBinding = Expression.Bind(item, property);
    //            memberBindingList.Add(memberBinding);
    //        }

    //        MemberInitExpression memberInitExpression = Expression.MemberInit(Expression.New(typeof(TOut)), memberBindingList.ToArray());
    //        Expression<Func<TIn, TOut>> lambda = Expression.Lambda<Func<TIn, TOut>>(memberInitExpression, new ParameterExpression[] { parameterExpression });

    //        return lambda.Compile();
    //    }

    //    public static TOut Trans(TIn tIn)
    //    {
    //        return cache(tIn);
    //    }

    //}
}
