using NLog;
using QuantaBasket.Core.Interfaces;
using QuantaBasket.Core.Messages;
using QuantaBasket.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace QuantaBasket.Basket
{
    sealed class QuantItem : IBasketService, IDisposable
    {
        private Action<AMessage> _messageProcessor;
        private readonly BasketEngine _basketEngine;
        private readonly ILogger _logger = LogManager.GetLogger("QuantItem");

        private readonly AsyncWorker<AMessage> _worker;

        public IQuant Quant { get; set; }

        public QuantItem(BasketEngine basketEngine)
        {
            _basketEngine = basketEngine;
            _worker = new AsyncWorker<AMessage>("QuantItemSender",
                 (m) =>
                 {
                     try
                     {
                         _messageProcessor?.Invoke(m);
                     }
                     catch (Exception ex)
                     {
                         _logger.Error(ex, $"Error processing message by Quant '{Quant.Name}'. Message: {m}");
                     }
                 },
                 () => SendMessage(new TimerMessage { DateTime = DateTime.Now }), 1000);
        }

        public void Dispose()
        {
            try
            {
                _logger.Debug($"{Quant?.Name}: Disposing");
                _worker.Dispose();
                Quant?.Dispose();
            }
            catch(Exception ex)
            {
                _logger.Error(ex);
            }
        }

        public void RegisterCallback(Action<AMessage> messageProcessor)
        {
            _messageProcessor = messageProcessor;
            _logger.Debug($"{Quant?.Name} Message processor registered");
        }

        public void SendMessage(AMessage message)
        {
            _logger.Trace($"Send message to quant '{Quant?.Name}': {message} ");
            _worker.AddItem(message);
        }

        public INewSignal CreateSignal()
        {
            return _basketEngine.CreateSignal(Quant?.Name);
        }

        public bool SendSignal(INewSignal signal)
        {
            try
            {
                _basketEngine.SendSignal(signal);
                return true;
            }
            catch(Exception ex)
            {
                _logger.Error(ex);
                return false;
            }
        }

        public ISignal GetTodaySignal(string signalId)
        {
            try
            {
                return _basketEngine.GetTodaySignal(signalId);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return null;
            }
        }

        public void CancelSignal(string signalId)
        {
            _basketEngine.CancelSignal(signalId);
        }
    }
}
