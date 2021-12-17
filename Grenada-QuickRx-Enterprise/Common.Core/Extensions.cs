using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Common.Core
{
    public static class Extensions
    {
        public static List<List<T>> Split<T>(this IList<T> source, int length)
        {
            return source
                .Select((x, i) => new { Index = i, Value = x })
                .GroupBy(x => x.Index / length)
                .Select(x => x.Select(v => v.Value).ToList())
                .ToList();
        }

        public static void EnableCollectionSynchronization(IEnumerable collection, object lockObject)
        {
            // Equivalent to .NET 4.5:
            // BindingOperations.EnableCollectionSynchronization(collection, lockObject);
            MethodInfo method = typeof(BindingOperations).GetMethod("EnableCollectionSynchronization", new Type[] { typeof(IEnumerable), typeof(object) });
            if (method != null)
            {
                method.Invoke(null, new object[] { collection, lockObject });
            }
        }


    }
}
