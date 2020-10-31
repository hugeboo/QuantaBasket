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
    public sealed class TradingSystemMock : ITradingSystem
    {
        private bool _connected;

        private Action<ErrorReportCode, string> _onErrorAction;
        private Action<TradeDTO> _onTradeAction;
        private Action<OrderStatusDTO> _onOrderStatusAction;

        private readonly ILogger _logger = LogManager.GetLogger("TradingSystemMock");
        private readonly IClock _clock;
        private ITradeSignal _currentSignal;

        public bool Connected => _connected;

        public TradingSystemMock(IClock clock)
        {
            _clock = clock ?? throw new ArgumentNullException(nameof(clock));
        }

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
            return Configuration.Instance;
        }

        public void RegisterCallback(Action<ErrorReportCode, string> processError)
        {
            _logger.Info("RegisterErrorProcessor");
            _onErrorAction = processError;
        }

        public void RegisterCallcback(Action<OrderStatusDTO> processOrderStatus)
        {
            _logger.Info("RegisterOrderStatusProcessor");
            _onOrderStatusAction = processOrderStatus;
        }

        public void RegisterCallback(Action<TradeDTO> processTrade)
        {
            _logger.Info("RegisterTradeProcessor");
            _onTradeAction = processTrade;
        }

        public void SaveConfiguration()
        {
            _logger.Info("SaveConfiguration");
            Configuration.Instance.Save();
        }

        public void SendSignal(ITradeSignal signal)
        {
            _logger.Info($"SendSignal '{signal}'");
            if (signal.SecCode == "GAZP")
            {
                if (_currentSignal == null)
                {
                    _currentSignal = signal;
                    var marketOrderId = new Random(Environment.TickCount).Next(1000, (int)Math.Pow(2, 30));
                    _onOrderStatusAction(new OrderStatusDTO
                    {
                        MarketOrderId = marketOrderId.ToString(),
                        MarketDateTime = _clock.Now.Value,
                        ClassCode = signal.ClassCode,
                        SecCode = signal.SecCode,
                        SignalId = signal.Id,
                        Status = SignalStatus.Open,
                    });
                }
                else
                {
                    _onOrderStatusAction(new OrderStatusDTO
                    {
                        MarketDateTime = _clock.Now.Value,
                        ClassCode = signal.ClassCode,
                        SecCode = signal.SecCode,
                        SignalId = signal.Id,
                        Status = SignalStatus.Rejected,
                        Text = $"Mock: First you need to cancel the signal with id '{_currentSignal.Id}'"
                    });
                }
            }
            else
            {
                Task.Factory.StartNew(() => SimpleSignalExecutor(signal));
            }
        }

        public void CancelSignal(ITradeSignal signal)
        {
            _logger.Info($"CancelSignal '{signal}'");
            if (_currentSignal != null && _currentSignal.Id == signal.Id)
            {
                _onOrderStatusAction(new OrderStatusDTO
                {
                    MarketDateTime = _clock.Now.Value,
                    ClassCode = signal.ClassCode,
                    SecCode = signal.SecCode,
                    SignalId = signal.Id,
                    Status = SignalStatus.Canceled,
                });
                _currentSignal = null;
            }
        }

        private void SimpleSignalExecutor(ITradeSignal signal)
        {
            _logger.Info($"Start SimpleSignalExecutor. Signal: {signal}");

            var marketOrderId = new Random(Environment.TickCount).Next(1000, (int)Math.Pow(2, 30));

            Thread.Sleep(500);
            _onOrderStatusAction(new OrderStatusDTO
            {
                MarketOrderId = marketOrderId.ToString(),
                MarketDateTime = _clock.Now.Value,
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
                MarketDateTime = _clock.Now.Value,
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
                MarketDateTime = _clock.Now.Value,
                ClassCode = signal.ClassCode,
                SecCode = signal.SecCode,
                SignalId = signal.Id,
                Status = SignalStatus.Completed,
            });
        }
    }
}
