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
using System.Threading.Tasks;

namespace QuantaBasket.Basket
{
    public sealed class BasketEngine : IBasketEngine
    {
        private readonly Dictionary<string, QuantItem> _quantas = new Dictionary<string, QuantItem>();
        private readonly ILogger _logger = LogManager.GetLogger("BasketEngine");

        private IL1QuotationProvider _quoteProvider;
        private IL1QuotationStore _quoteStore;

        public BasketEngine()
        {
            Init();
        }

        private void Init()
        {
            _logger.Info("Initialize BasketEngine");
            _logger.Debug("Create L1QuotationStore");
            _quoteStore = new SQLiteL1QuotationStore();

            _logger.Debug("Create L1QuotationProvider");
            _quoteProvider = new QLuaL1QuotationProvider(_quoteStore);
            _quoteProvider.RegisterQuotationProcessor(ProcessQuotation);

            RegisterQuantas();
            InitQuantas();
        }

        private void ProcessQuotation(IEnumerable<L1Quotation> quotations)
        {
            foreach(var q in quotations)
            {
                foreach(var item in _quantas.Values)
                {
                    if (item.Quant.Securities.Contains(q.Security))
                    {
                        item.SendMessage(new L1QuotationsMessage() { Quotations = new[] { q } });
                    }
                }
            }
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

        public void Dispose()
        {
            Stop();
            _logger.Debug("Disposing");
            _quoteProvider.Dispose();
            _logger.Debug("Disposing quantos");
            _quantas.Values.ForEach(q => q.Quant.Dispose());
        }

        public void Start()
        {
            _logger.Debug("Starting");
            _quoteProvider.Connect();
            _quantas.Values.ForEach(q => q.MessageProcessor?.Invoke(new StartMessage()));
        }

        public void Stop()
        {
            _logger.Debug("Stopping");
            _quantas.Values.ForEach(q => q.MessageProcessor?.Invoke(new StopMessage()));
        }
    }
}
