using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuantaBasket.Core.Exceptions
{
    /// <summary>
    /// Потеря соединения с торговой системой
    /// </summary>
    public sealed class ConnectionLostException : Exception
    {
        public ConnectionLostException(string message, Exception innerException) :
            base(message, innerException)
        {
        }
    }
}
