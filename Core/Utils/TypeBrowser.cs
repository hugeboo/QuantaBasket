using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuantaBasket.Core.Utils
{
    public static class TypeBrowser
    {
        public static IEnumerable<Type> Browse(string interfaceName)
        {
            var lst = new List<Type>();
            foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                lst.AddRange(asm.GetTypes()
                     .Where(t => t.IsClass && !t.IsAbstract && t.IsPublic && t.GetInterface(interfaceName) != null));
            }            
            return lst;
        }
    }
}
