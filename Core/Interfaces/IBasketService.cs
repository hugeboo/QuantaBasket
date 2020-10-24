using QuantaBasket.Core.Contracts;
using QuantaBasket.Core.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuantaBasket.Core.Interfaces
{
    public interface IBasketService
    {
        void RegisterMessageProcessor(Action<AMessage> messageProcessor);
        IQuantSignal CreateSignal();
        void SendSignal(IQuantSignal signal);
    }
}
