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
    public sealed class BasketEngine
    {
        private readonly Dictionary<string, QuantItem> _quantas = new Dictionary<string, QuantItem>();

        private readonly ILogger _logger = LogManager.GetLogger("BasketEngine");

        public BasketEngine()
        {
            RegisterQuantas();
            InitQuantas(); //пока здесь временно
        }

        private void RegisterQuantas()
        {
            var types = QuantBrowser.Browse();
            foreach(var t in types)
            {
                var q = Activator.CreateInstance(t) as IQuant;
                var item = new QuantItem { Quant = q };
                _quantas[item.Quant.Name] = item;
                item.Quant.BasketService = item;
            }
        }

        private void InitQuantas()
        {
            foreach(var q in _quantas.Values)
            {
                q.Quant.Init();
            }
        }
    }
}
