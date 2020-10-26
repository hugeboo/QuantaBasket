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
        //private readonly object _syncObj = new object();
        //private readonly List<AMessage> _messageQueue = new List<AMessage>();
        //private readonly AutoResetEvent _newMessageEvent = new AutoResetEvent(false);
        //private readonly Thread _threadSender;

        private readonly AsyncWorker<AMessage> _worker;

        public IQuant Quant { get; set; }

        public QuantItem(BasketEngine basketEngine)
        {
            _basketEngine = basketEngine;
            //_threadSender = new Thread(ThreadSenderProc);
            //_threadSender.Start();

            _worker = new AsyncWorker<AMessage>("QuantItemSender",
                (m) => _messageProcessor?.Invoke(m),
                () => SendMessage(new TimerMessage { DateTime = DateTime.Now }),
                1000);
        }

        public void Dispose()
        {
            try
            {
                _logger.Debug($"{Quant?.Name}: Disposing");
                //_threadSender.Abort();
                _worker.Dispose();
                Quant?.Dispose();
            }
            catch(Exception ex)
            {
                _logger.Error(ex);
            }
        }

        public void RegisterMessageProcessor(Action<AMessage> messageProcessor)
        {
            _messageProcessor = messageProcessor;
            _logger.Debug($"{Quant?.Name} Message processor registered");
        }

        public void SendMessage(AMessage message)
        {
            _logger.Trace($"{Quant?.Name}: Send message: {message}");
            //lock (_syncObj) _messageQueue.Add(message);
            //_newMessageEvent.Set();
            _worker.AddItem(message);
        }
        
        //private void ThreadSenderProc(object state)
        //{
        //    try
        //    {
        //        while (true)
        //        {
        //            if (!_newMessageEvent.WaitOne(1000))
        //            {
        //                SendMessage(new TimerMessage { DateTime = DateTime.Now });
        //                continue;
        //            }

        //            var lst = new List<AMessage>();
        //            lock (_syncObj)
        //            {
        //                lst.AddRange(_messageQueue);
        //                _messageQueue.Clear();
        //            }
        //            foreach (var m in lst)
        //            {
        //                try
        //                {
        //                    _messageProcessor?.Invoke(m);
        //                }
        //                catch (Exception ex)
        //                {
        //                    _logger.Error(ex, $"Error send message to {Quant?.Name}. Message: {m}");
        //                }
        //            }
        //        }
        //    }
        //    catch (ThreadAbortException) 
        //    { 
        //    }
        //    catch(Exception ex)
        //    {
        //        _logger.Error(ex);
        //    }
        //}

        public IQuantSignal CreateSignal()
        {
            return _basketEngine.CreateSignal(Quant?.Name);
        }

        public void SendSignal(IQuantSignal signal)
        {
            _basketEngine.SendSignal(signal);
        }
    }
}
