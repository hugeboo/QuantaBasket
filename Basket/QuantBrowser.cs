using QuantaBasket.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace QuantaBasket.Basket
{
    internal static class QuantBrowser
    {
        public static IEnumerable<Type> Browse()
        {
            var lst = new List<Type>();
            string quantasPath;
#if DEBUG
            quantasPath = Properties.Settings.Default.DebugQuantasPath;
#else
            quantasPath = Properties.Settings.Default.ReleaseQuantasPath;
#endif

            var files = Directory.GetFiles(quantasPath, "*Quant.dll", SearchOption.AllDirectories);
            foreach(var file in files.Where(f => !f.Contains("\\obj\\")))
            {
                var asm = Assembly.LoadFile(file);
                var tt = asm.GetTypes()
                    .Where(t => t.IsClass && !t.IsAbstract && t.IsPublic && t.GetInterface("IQuant") !=null)
                    .ToArray();
                if (tt.Length == 1)
                {
                    lst.Add(tt[0]);
                }
            }

            return lst;
        }
    }
}
