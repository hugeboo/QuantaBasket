using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuantaBasket.Core.Utils
{
    public sealed class EventArgs<T> : EventArgs
    {
        public T Data { get; set; }

        public EventArgs()
        {
        }

        public EventArgs(T data)
        {
            Data = data;
        }
    }
}
