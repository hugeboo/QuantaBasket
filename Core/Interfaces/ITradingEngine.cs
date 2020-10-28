using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuantaBasket.Core.Interfaces
{
    /// <summary>
    /// Движок для обработки сигналов и взаимодействия с торговой системой
    /// </summary>
    public interface ITradingEngine : IDisposable
    {
        /// <summary>
        /// Старт системы (подклчение к ТС)
        /// </summary>
        void Start();

        /// <summary>
        /// Остановка системы
        /// </summary>
        void Stop();

        bool TradingSystemConnected { get; }

        ITradingSystem TradingSystem { get; }

        ITradingStore TradingStore { get; }

        /// <summary>
        /// Создать новый торговый сигнал
        /// У нового сигнала заполнены некоторые обязательные поля, остальные заполняет квант
        /// </summary>
        /// <param name="quantName">Имя кванта</param>
        /// <returns>Сигнал для дальнейшего заполнения и отправки</returns>
        INewSignal CreateSignal(string quantName);

        /// <summary>
        /// Оправить сигнал в тороговую систему
        /// </summary>
        /// <param name="signal">Тороговый сигнал</param>
        void SendSignal(INewSignal signal);

        ISignal GetTodaySignal(string signalId);

        void CancelSignal(string signalId);
    }
}
