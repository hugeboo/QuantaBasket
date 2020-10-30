using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuantaBasket.Core.Interfaces
{
    public interface IStreamDataStore : IDisposable
    {
        IStreamDataChannel<T> OpenOrCreateChannel<T>(string channelName);
    }

    public interface IStreamDataChannel<T> : IDisposable
    {
        void Write(T data);
    }
}
