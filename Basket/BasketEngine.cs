using NLog;
using QuantaBasket.Components.QLuaL1QuotationProvider;
using QuantaBasket.Components.SQLiteL1QuotationStore;
using QuantaBasket.Core.Contracts;
using QuantaBasket.Core.Extensions;
using QuantaBasket.Core.Interfaces;
using QuantaBasket.Core.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Trader;

namespace QuantaBasket.Basket
{
    public sealed class BasketEngine : IBasketEngine
    {
        private readonly Dictionary<string, QuantItem> _quantas = new Dictionary<string, QuantItem>();
        private readonly ILogger _logger = LogManager.GetLogger("BasketEngine");
        private Timer _timer;

        private IL1QuotationProvider _quoteProvider;
        private IL1QuotationStore _quoteStore;

        private ITradingEngine _tradingEngine;

        public BasketEngine()
        {
            Init();
        }

        /// <summary>
        /// Создать новый торговый сигнал
        /// У нового сигнала заполнены некоторые обязательные поля, остальные заполняет квант
        /// </summary>
        /// <param name="quantName">Имя кванта</param>
        /// <returns>Сигнал для дальнейшего заполнения и отправки</returns>
        internal IQuantSignal CreateSignal(string quantName)
        {
            return _tradingEngine.CreateSignal(quantName);
        }

        /// <summary>
        /// Оправить сигнал в тороговую систему
        /// </summary>
        /// <param name="signal">Тороговый сигнал</param>
        internal void SendSignal(IQuantSignal signal)
        {
            _tradingEngine.SendSignal(signal);
        }

        private void Init()
        {
            try
            {
                _logger.Info("Initialize BasketEngine");

                _logger.Debug("Create L1QuotationStore");
                _quoteStore = new SQLiteL1QuotationStore();

                _logger.Debug("Create L1QuotationProvider");
                _quoteProvider = new QLuaL1QuotationProvider(_quoteStore);
                _quoteProvider.RegisterQuotationProcessor(ProcessQuotation);
                _quoteProvider.RegisterErrorProcessor(ProcessError);

                _logger.Debug("Creaete TradingEngine");
                _tradingEngine = new TradingEngine();

                RegisterQuantas();
                InitQuantas();
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                throw;
            }
        }

        private void ProcessError(ErrorReportCode errorCode, string message)
        {
            SendAllQuanas(new ErrorMessage { ErrorCode = errorCode, Message = message });
            Stop();
        }

        private void ProcessQuotation(IEnumerable<L1Quotation> quotations)
        {
            quotations.ForEach(q =>
                _quantas.Values.ForEach(item =>
                {
                    if (item.Quant.Securities.Contains(q.Security))
                    {
                        item.SendMessage(new L1QuotationsMessage() { Quotations = new[] { q } });
                    }
                })
            );
        }

        private void RegisterQuantas()
        {
            QuantBrowser.Browse().ForEach(t =>
                {
                    var q = Activator.CreateInstance(t) as IQuant;
                    var item = new QuantItem(this) { Quant = q };
                    _quantas[item.Quant.Name] = item;
                    item.Quant.BasketService = item;
                    _logger.Info($"Quant '{item.Quant.Name}' registered");
                });
        }

        private void InitQuantas()
        {
            _logger.Debug("Initializing quantos");
            _quantas.Values.ForEach(q => q.Quant.Init());
        }

        private void TimerProc(object state)
        {
            SendAllQuanas(new TimerMessage { DateTime = DateTime.Now });
        }

        private void SendAllQuanas(AMessage m)
        {
            _quantas.Values.ForEach(q => q.SendMessage(m));
        }

        public void Dispose()
        {
            try
            {
                Stop();
                _logger.Debug("Disposing");
                _timer?.Dispose();
                _quoteProvider.Dispose();
                _tradingEngine.Dispose();
                _logger.Debug("Disposing quantos");
                _quantas.Values.ForEach(q => q.Quant.Dispose());
            }
            catch(Exception ex)
            {
                _logger.Error(ex);
            }
        }

        public void Start()
        {
            try
            {
                _logger.Debug("Starting");
                _quoteProvider.Connect();
                _tradingEngine.Start();
                SendAllQuanas(new StartMessage());
                _logger.Debug("Start timer for 1sec");
                _timer?.Dispose();
                _timer = new Timer(TimerProc, null, 1000, 1000);

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
                SendAllQuanas(new StopMessage());
                _timer?.Dispose();
                _tradingEngine.Stop();
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                throw;
            }
        }
    }
}
