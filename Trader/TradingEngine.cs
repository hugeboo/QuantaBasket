using NLog;
using QuantaBasket.Components.SQLiteTradingStore;
using QuantaBasket.Components.TradingSystemMock;
using QuantaBasket.Core.Contracts;
using QuantaBasket.Core.Interfaces;
using QuantaBasket.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace QuantaBasket.Trader
{
    internal sealed class TradingEngine : ITradingEngine, IHaveConfiguration
    {
        private readonly ILogger _logger = LogManager.GetLogger("TradingEngine");

        private ITradingStore _tradingStore;
        private ITradingSystem _tradingSystem;

        private int _nextId;
        private AsyncWorker<SignalDTO> _sendOrderWorker;
        private AsyncWorker<object> _reportOrderWorker;

        public bool TradingSystemConnected => _tradingSystem?.Connected ?? false;

        public ITradingSystem TradingSystem => _tradingSystem;

        public ITradingStore TradingStore => _tradingStore;

        public TradingEngine()
        {
            Init();
        }

        public INewSignal CreateSignal(string quantName)
        {
            var id = Interlocked.Increment(ref _nextId);
            var signal = new SignalDTO 
            { 
                Id = id.ToString(), 
                QuantName = quantName,
                CreatedTime = DateTime.Now,
                Status = SignalStatus.New
            };
            _logger.Trace($"Signal created: {signal}");
            return signal;
        }

        public void SendSignal(INewSignal signal)
        {
            var s = signal as SignalDTO;
            if (s == null) throw new InvalidCastException("Invalid Type for the IQuantSignal");
            if (!ValidateNewSignal(s, out string err))
            {
                throw new InvalidOperationException(err);
            }
            _sendOrderWorker.AddItem(s);
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
                _sendOrderWorker?.Dispose();
                _reportOrderWorker?.Dispose();
                _tradingSystem?.Dispose();
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }

        public object GetConfiguration()
        {
            return Configuration.Default;
        }

        public void SaveConfiguration()
        {
            Configuration.Default.Save();
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

                _logger.Debug("Start ReportOrderWorker");
                _reportOrderWorker = new AsyncWorker<object>("ReportOrder", ReportOrderWorkerProc);

                _logger.Debug("Start SendOrderWorker");
                _sendOrderWorker = new AsyncWorker<SignalDTO>("SendSignal", SendSignalWorkerProc);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                throw;
            }
        }

        private static bool ValidateNewSignal(SignalDTO signal, out string errorMessage)
        {
            var lstErrors = new List<string>();

            if (string.IsNullOrWhiteSpace(signal.ClassCode)) lstErrors.Add("ClassCode must be filled");
            if (string.IsNullOrWhiteSpace(signal.SecCode)) lstErrors.Add("SecCode must be filled");
            if (signal.Qtty <= 0) lstErrors.Add("Qtty must be > 0");
            if (signal.Price < 0m) lstErrors.Add(signal.PriceType == PriceType.Market ? "Price must be = 0" : "Price must be > 0");
            if (signal.Price == 0m && signal.PriceType != PriceType.Market) lstErrors.Add("Price must be > 0");

            if (lstErrors.Count > 0)
            {
                errorMessage = string.Join(", ", lstErrors);
                return false;
            }
            else
            {
                errorMessage = null;
                return true;
            }
        }

        private static OrderStatusDTO MakeOrderStatusDTO(SignalDTO signal)
        {
            return new OrderStatusDTO
            {
                SignalId = signal.Id,
                ClassCode = signal.ClassCode,
                SecCode = signal.SecCode,
                MarketOrderId = signal.MarketOrderId,
                Status = signal.Status
            };
        }

        public static bool UpdateSignal(SignalDTO signal, OrderStatusDTO orderStatus)
        {
            if (orderStatus.SignalId != signal.Id)
                throw new InvalidOperationException($"orderStatus.SignalId({orderStatus.SignalId}) != Id({signal.Id})");
            if (orderStatus.ClassCode != signal.ClassCode)
                throw new InvalidOperationException($"orderStatus.ClassCode({orderStatus.ClassCode}) != Id({signal.ClassCode})");
            if (orderStatus.SecCode != signal.SecCode)
                throw new InvalidOperationException($"orderStatus.SecCode({orderStatus.SecCode}) != Id({signal.SecCode})");

            bool updated = false;

            if (!string.IsNullOrWhiteSpace(orderStatus.MarketOrderId))
            {
                signal.MarketOrderId = orderStatus.MarketOrderId;
                updated = true;
            }
            if (!signal.Status.IsFinished() && orderStatus.Status != signal.Status)
            {
                signal.Status = orderStatus.Status;
                updated = true;
            }
            if (updated)
            {
                signal.LastUpdateTime = DateTime.Now;
            }

            return updated;
        }

        public static void UpdateSignal(SignalDTO signal, TradeDTO trade)
        {
            if (trade.SignalId != signal.Id)
                throw new InvalidOperationException($"trade.SignalId({trade.SignalId}) != Id({signal.Id})");
            if (trade.ClassCode != signal.ClassCode)
                throw new InvalidOperationException($"trade.ClassCode({trade.ClassCode}) != Id({signal.ClassCode})");
            if (trade.SecCode != signal.SecCode)
                throw new InvalidOperationException($"trade.SecCode({trade.SecCode}) != Id({signal.SecCode})");

            if (signal.ExecQtty == 0)
            {
                signal.AvgPrice = trade.Price;
                signal.ExecQtty = trade.Qtty;
            }
            else
            {
                var avgPrice = (signal.AvgPrice * signal.ExecQtty + trade.Price * trade.Qtty) / 
                    (signal.ExecQtty + trade.Qtty);

                signal.AvgPrice = avgPrice;
                signal.ExecQtty += trade.Qtty;
            }

            signal.LastUpdateTime = DateTime.Now;
        }

        private void ProcessError(ErrorReportCode errorCode, string message)
        {
            _logger.Error($"Trading System error. {errorCode}: {message}");
            Stop();
        }

        private void ProcessTrade(TradeDTO trade)
        {
            _reportOrderWorker.AddItem(trade);
        }

        private void ProcessOrderStatus(OrderStatusDTO orderStatus)
        {
            _reportOrderWorker.AddItem(orderStatus);
        }

        private void SendSignalWorkerProc(SignalDTO signal)
        {
            try
            {
                signal.Status = SignalStatus.Sent;
                signal.LastUpdateTime = DateTime.Now;
                _tradingStore.Insert(signal);
                _tradingSystem.SendSignal(signal);
            } 
            catch(Exception ex)
            {
                _logger.Error(ex);

                var os = MakeOrderStatusDTO(signal);
                os.Status = SignalStatus.Rejected;
                os.Text = $"Rejected by TradingEngine. Error: {ex.Message}";
                _reportOrderWorker.AddItem(os);
            }
        }

        private void ReportOrderWorkerProc(object obj)
        {
            try
            {
                switch(obj)
                {
                    case TradeDTO trade:
                        {
                            _tradingStore.Insert(trade);
                            var signal = _tradingStore.GetSignalByIdAndDate(trade.SignalId, DateTime.Today) as SignalDTO;
                            if (signal == null)
                                throw new InvalidOperationException($"Update by trade - Cannot find signal with Id {trade.SignalId}");
                            UpdateSignal(signal, trade);
                            _tradingStore.Update(signal);
                        }
                        break;

                    case OrderStatusDTO orderStatus:
                        {
                            var signal = _tradingStore.GetSignalByIdAndDate(orderStatus.SignalId, DateTime.Today) as SignalDTO;
                            if (signal == null)
                                throw new InvalidOperationException($"Update by orderStatus - Cannot find signal with Id {orderStatus.SignalId}");
                            UpdateSignal(signal, orderStatus);
                            _tradingStore.Update(signal);
                        }
                        break;
                }
            }
            catch(Exception ex)
            {
                _logger.Error(ex, $"Cannot process object: {obj}. Error: {ex.Message}");
            }
        }
    }
}
