using System;
using System.Collections.Generic;
using System.Text;

namespace System.Linq
{
    public static class LinqExtensions
    {
        public static void Map<T>(this IEnumerable<T> input, Action<T> function)
        {
            foreach (var item in input)
            {
                function(item);
            }
        }
    }
}
