using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuantaBasket.Core.Exceptions
{
    /// <summary>
    /// Ошибка при разборе сообщения от торговой системы
    /// </summary>
    public sealed class BadMessageException : Exception
    {
        public BadMessageException(string message, Exception innerException) :
            base(message, innerException)
        {
        }
    }
}
