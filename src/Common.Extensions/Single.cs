using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Extensions
{
    public class Single<T> where T : class, new()
    {
        public static T Instance { get; } = new T();
    }
}
