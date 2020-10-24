using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuantaBasket.Core.Utils
{
    /// <summary>
    /// Поиск типов, реализующих определенный интерфейс во всех сборках, загруженных в домен приложения
    /// </summary>
    public static class TypeBrowser
    {
        /// <summary>
        /// Найтти тиипы, реализующие интерфейс
        /// Типы должны быть не абстрактными публичными классами
        /// </summary>
        /// <param name="interfaceName">Имя интерфейса</param>
        /// <returns></returns>
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
