using QuantaBasket.Core.Interfaces;
using QuantaBasket.Core.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuantaBasket.Basket
{
    sealed class QuantItem : IBasketService
    {
        private Action<AMessage> _messageProcessor;

        public IQuant Quant { get; set; }
        public Action<AMessage> MessageProcessor => _messageProcessor;

        public void RegisterMessageProcessor(Action<AMessage> messageProcessor)
        {
            _messageProcessor = messageProcessor;
        }
    }
}
