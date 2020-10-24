using QuantaBasket.Core.Contracts;
using QuantaBasket.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuantaBasket.Components.TradingSystemMock
{
    public sealed class TradingSystemMock : ITradingSystem
    {
        private bool _connected;

        private Action<ErrorReportCode, string> _onErrorAction;
        private Action<TradeDTO> _onTradeAction;
        private Action<OrderStatusDTO> _onOrderStatusAction;

        public bool Connected => _connected;

        public void Connect()
        {
            _connected = true;
        }

        public void Disconnect()
        {
            _connected = false;
        }

        public void Dispose()
        {
            Disconnect();
        }

        public void RegisterErrorProcessor(Action<ErrorReportCode, string> processError)
        {
            _onErrorAction = processError;
        }

        public void RegisterOrderStatusProcessor(Action<OrderStatusDTO> processOrderStatus)
        {
            _onOrderStatusAction = processOrderStatus;
        }

        public void RegisterTradeProcessor(Action<TradeDTO> processTrade)
        {
            _onTradeAction = processTrade;
        }

        public void SendSignal(ITradeSignal signal)
        {
            throw new NotImplementedException();
        }
    }
}
