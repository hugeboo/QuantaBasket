using NLog;
using QuantaBasket.Components.SQLiteTradingStore;
using QuantaBasket.Components.TradingSystemMock;
using QuantaBasket.Core.Contracts;
using QuantaBasket.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Trader
{
    public sealed class TradingEngine : ITradingEngine
    {
        private readonly ILogger _logger = LogManager.GetLogger("TradingEngine");

        private ITradingStore _tradingStore;
        private ITradingSystem _tradingSystem;

        private int _nextId;

        public bool TradingSystemConnected => _tradingSystem?.Connected ?? false;

        public TradingEngine()
        {
            Init();
        }

        public IQuantSignal CreateSignal(string quantName)
        {
            var id = Interlocked.Increment(ref _nextId);
            var signal = new Signal(id.ToString(), quantName);
            _logger.Trace($"Signal created: {signal}");
            return signal;
        }

        public void SendSignal(IQuantSignal signal)
        {
            var s = signal as Signal;
            if (s == null) throw new InvalidCastException("Invalid Type for the IQuantSignal");
            if (!s.PrimaryValidate(out string err))
            {
                throw new InvalidOperationException(err);
            }

            _tradingStore?.Insert(s.ToSignalDTO());

            //...
        }

        public void Start()
        {
            try
            {
                _logger.Debug("Starting");
                _tradingSystem.Connect();
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                throw;
            }
        }

        public void Stop()
        {
            try
            {
                _logger.Debug("Stopping");
                _tradingSystem.Disconnect();
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                throw;
            }
        }

        public void Dispose()
        {
            try
            {
                Stop();
                _logger.Debug("Disposing");
                _tradingSystem.Dispose();
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }

        private void Init()
        {
            try
            {
                _logger.Info("Initialize TradingEngine");

                _nextId = Environment.TickCount;

                _logger.Debug("Create TradingStore");
                _tradingStore = new SQLiteTradingStore();

                _logger.Debug("Create TradingSystem");
                _tradingSystem = new TradingSystemMock();
                _tradingSystem.RegisterErrorProcessor(ProcessError);
                _tradingSystem.RegisterTradeProcessor(ProcessTrade);
                _tradingSystem.RegisterOrderStatusProcessor(ProcessOrderStatus);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                throw;
            }
        }

        private void ProcessError(ErrorReportCode errorCode, string message)
        {
            Stop();
        }

        private void ProcessTrade(TradeDTO trade)
        {

        }

        private void ProcessOrderStatus(OrderStatusDTO orderStatus)
        {

        }
    }
}
