using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuantaBasket.Core.Exceptions
{
    /// <summary>
    /// Ошибка траспортного уровня при взаимодействии с торговой системой
    /// </summary>
    public sealed class TransportException : Exception
    {
        public TransportException(string message, Exception innerException) :
            base(message, innerException)
        {
        }
    }
}
