using QuantaBasket.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuantaBasket.Core.Interfaces
{
    public interface ITradingSystem : IErrorReporter, IDisposable, IHaveConfiguration
    {
        /// <summary>
        /// Зарегистрировать обработчик трейдов
        /// </summary>
        /// <param name="processTrade"></param>
        void RegisterCallback(Action<TradeDTO> processTrade);

        /// <summary>
        /// Зарегистрировать обработчик статусов ордеров
        /// </summary>
        /// <param name="processOrderStatus"></param>
        void RegisterCallcback(Action<OrderStatusDTO> processOrderStatus);

        /// <summary>
        /// Подключиться к тороговой системе
        /// </summary>
        void Connect();

        /// <summary>
        /// Отключиться от торговой системы
        /// </summary>
        void Disconnect();

        /// <summary>
        /// Статус подключения
        /// </summary>
        bool Connected { get; }

        /// <summary>
        /// Сгенерить по сигналу рыночный ордер и отправить его в тороговую систему
        /// </summary>
        void SendSignal(ITradeSignal signal);

        void CancelSignal(ITradeSignal signal);
    }
}
