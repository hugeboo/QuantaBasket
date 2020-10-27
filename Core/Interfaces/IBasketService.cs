using QuantaBasket.Core.Contracts;
using QuantaBasket.Core.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuantaBasket.Core.Interfaces
{
    /// <summary>
    /// Сервис основного движка, предоставляемы в распоряжение кванта
    /// </summary>
    public interface IBasketService
    {
        /// <summary>
        /// Зарегистрировать обработчик сообщений, поступающих кванту
        /// </summary>
        /// <param name="messageProcessor"></param>
        void RegisterMessageProcessor(Action<AMessage> messageProcessor);

        /// <summary>
        /// Создать новый торговый сигнал
        /// У нового сигнала заполнены некоторые обязательные поля, остальные заполняет квант
        /// </summary>
        /// <returns>Сигнал для дальнейшего заполнения и отправки</returns>
        INewSignal CreateSignal();

        /// <summary>
        /// Оправить сигнал в тороговую систему
        /// </summary>
        /// <returns>False означает, что сигнал не прошел внутреннюю валидацию. Это явный признак ошибки в кванте</returns>
        bool SendSignal(INewSignal signal);

        ISignal GetTodaySignal(string signalId);
    }
}
