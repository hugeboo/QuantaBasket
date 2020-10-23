using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuantaBasket.Core.Extensions
{
    public static class Enumerable
    {
        public static void ForEach<T>(this IEnumerable<T> lst, Action<T> action)
        {
            foreach(var it in lst)
            {
                action(it);
            }
        }
    }
}
