using NLog;
using QuantaBasket.Core.Interfaces;
using QuantaBasket.Core.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuantaBasket.Basket
{
    sealed class QuantItem : IBasketService, IDisposable
    {
        private Action<AMessage> _messageProcessor;
        private readonly BasketEngine _engine;

        private readonly ILogger _logger = LogManager.GetLogger("QuantItem");

        public IQuant Quant { get; set; }
        public Action<AMessage> MessageProcessor => _messageProcessor;

        public QuantItem(BasketEngine engine)
        {
            _engine = engine;
        }

        public void Dispose()
        {
            _logger.Debug($"{Quant?.Name}: Disposing");
            Quant?.Dispose();
        }

        public void RegisterMessageProcessor(Action<AMessage> messageProcessor)
        {
            _messageProcessor = messageProcessor;
            _logger.Debug($"{Quant?.Name} Message processor registered");
        }

        public void SendMessage(AMessage message)
        {
            _logger.Trace($"{Quant?.Name}: Send message: {message}");
            _messageProcessor?.Invoke(message);
        }
    }
}
