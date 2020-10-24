using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuantaBasket.Core.Interfaces
{
    /// <summary>
    /// Основной движок обеспечивающий инциализацию и работу квантов
    /// </summary>
    public interface IBasketEngine : IDisposable
    {
        /// <summary>
        /// Старт системы (подклчение к ТСб подписка на котировки, включение таймеров и т.д.)
        /// </summary>
        void Start();

        /// <summary>
        /// Остановка системы
        /// </summary>
        void Stop();

        /// <summary>
        /// Создать новый торговый сигнал
        /// У нового сигнала заполнены некоторые обязательные поля, остальные заполняет квант
        /// </summary>
        /// <param name="quantName">Имя кванта</param>
        /// <returns>Сигнал для дальнейшего заполнения и отправки</returns>
        IQuantSignal CreateSignal(string quantName);

        /// <summary>
        /// Оправить сигнал в тороговую систему
        /// </summary>
        /// <param name="signal">Тороговый сигнал</param>
        void SendSignal(IQuantSignal signal);
    }
}
