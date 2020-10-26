using NLog;
using QuantaBasket.Core.Contracts;
using QuantaBasket.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace QuantaBasket.Components.TradingSystemMock
{
    public sealed class TradingSystemMock : ITradingSystem, IHaveConfiguration
    {
        private bool _connected;

        private Action<ErrorReportCode, string> _onErrorAction;
        private Action<TradeDTO> _onTradeAction;
        private Action<OrderStatusDTO> _onOrderStatusAction;

        private readonly ILogger _logger = LogManager.GetLogger("TradingSystemMock");

        public bool Connected => _connected;

        public void Connect()
        {
            _logger.Info("Connect");
            _connected = true;
        }

        public void Disconnect()
        {
            _logger.Info("Disconnect");
            _connected = false;
        }

        public void Dispose()
        {
            _logger.Info("Dispose");
            Disconnect();
        }

        public object GetConfiguration()
        {
            _logger.Info("GetConfiguration");
            return Configuration.Default;
        }

        public void RegisterErrorProcessor(Action<ErrorReportCode, string> processError)
        {
            _logger.Info("RegisterErrorProcessor");
            _onErrorAction = processError;
        }

        public void RegisterOrderStatusProcessor(Action<OrderStatusDTO> processOrderStatus)
        {
            _logger.Info("RegisterOrderStatusProcessor");
            _onOrderStatusAction = processOrderStatus;
        }

        public void RegisterTradeProcessor(Action<TradeDTO> processTrade)
        {
            _logger.Info("RegisterTradeProcessor");
            _onTradeAction = processTrade;
        }

        public void SaveConfiguration()
        {
            _logger.Info("SaveConfiguration");
            Configuration.Default.Save();
        }

        public void SendSignal(ITradeSignal signal)
        {
            Task.Factory.StartNew(() => SimpleSignalExecutor(signal));
        }

        private void SimpleSignalExecutor(ITradeSignal signal)
        {
            _logger.Info($"Start SimpleSignalExecutor. Signal: {signal}");

            var marketOrderId = new Random(Environment.TickCount).Next(1000, (int)Math.Pow(2, 30));

            Thread.Sleep(500);
            _onOrderStatusAction(new OrderStatusDTO
            {
                MarketOrderId = marketOrderId.ToString(),
                MarketDateTime = DateTime.Now,
                ClassCode = signal.ClassCode,
                SecCode = signal.SecCode,
                SignalId = signal.Id,
                Status = SignalStatus.Open,
            });

            Thread.Sleep(500);
            _onTradeAction(new TradeDTO 
            {
                MarketTradeId = (marketOrderId + 1).ToString(),
                MarketOrderId = marketOrderId.ToString(),
                MarketDateTime = DateTime.Now,
                ClassCode = signal.ClassCode,
                SecCode = signal.SecCode,
                Side = signal.Side,
                Qtty = signal.Qtty,
                Price = signal.Price,
                SignalId = signal.Id
            });

            Thread.Sleep(500);
            _onOrderStatusAction(new OrderStatusDTO
            {
                MarketOrderId = marketOrderId.ToString(),
                MarketDateTime = DateTime.Now,
                ClassCode = signal.ClassCode,
                SecCode = signal.SecCode,
                SignalId = signal.Id,
                Status = SignalStatus.Completed,
            });
        }
    }
}
