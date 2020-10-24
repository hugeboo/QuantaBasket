using NLog;
using QuantaBasket.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trader
{
    public sealed class TradingEngine : ITradingEngine
    {
        private ILogger _logger = LogManager.GetLogger("TradingEngine");

        public IQuantSignal CreateSignal(string quantName)
        {
            var signal = new Signal(quantName);
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

            //...
        }
    }
}
