using QuantaBasket.Core.Contracts;
using QuantaBasket.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuantaBasket.Core.Messages
{
    public sealed class SignalChangedMessage : AMessage
    {
        public ISignal Signal { get; set; }
        public OrderStatusDTO OrderStatusDTO { get; set; }
        public TradeDTO TradeDTO { get; set; }
    }
}
