using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuantaBasket.Core.Exceptions
{
    public sealed class BadMessageException : Exception
    {
        public BadMessageException(string message, Exception innerException) :
            base(message, innerException)
        {
        }
    }
}
