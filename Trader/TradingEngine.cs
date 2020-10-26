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

namespace QuantaBasket.Trader
{
    public sealed class TradingEngine : ITradingEngine, IHaveConfiguration
    {
        private readonly ILogger _logger = LogManager.GetLogger("TradingEngine");

        private ITradingStore _tradingStore;
        private ITradingSystem _tradingSystem;

        private int _nextId;


        public bool TradingSystemConnected => _tradingSystem?.Connected ?? false;

        public ITradingSystem TradingSystem => _tradingSystem;

        public ITradingStore TradingStore => _tradingStore;

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
                if (!_tradingSystem.Connected)_tradingSystem.Connect();
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
                if(_tradingSystem.Connected) _tradingSystem.Disconnect();
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
            _logger.Error($"Trading System error. {errorCode}: {message}");
            Stop();
        }

        private void ProcessTrade(TradeDTO trade)
        {

        }

        private void ProcessOrderStatus(OrderStatusDTO orderStatus)
        {

        }

        public object GetConfiguration()
        {
            return Configuration.Default;
        }

        public void SaveConfiguration()
        {
            Configuration.Default.Save();
        }
    }
}
