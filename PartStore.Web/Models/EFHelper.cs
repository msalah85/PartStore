using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PartStore.Web.Models
{
    public static class EFHelper
    {
        public static IEnumerable<T> ForEach<T>(IEnumerable<T> xs, Action<T> f)
        {
            foreach (var x in xs)
            {
                f(x); yield return x;
            }
        }
    }
}
