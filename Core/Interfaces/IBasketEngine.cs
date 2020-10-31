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
    public interface IBasketEngine : IDisposable, IHaveConfiguration, IMessageSender, IClock
    {
        /// <summary>
        /// Старт системы (подклчение к ТС, подписка на котировки и т.д.)
        /// </summary>
        void Start();

        /// <summary>
        /// Остановка системы
        /// </summary>
        void Stop();

        /// <summary>
        /// Признак активности движка
        /// При внутренних ошибках он может самопроизвольно останавливаться
        /// </summary>
        bool Started { get; }

        /// <summary>
        /// Возращает список имен квантов в системе
        /// </summary>
        string[] GetQuantasNames();

        /// <summary>
        /// Возвращает квант по имени
        /// </summary>
        /// <param name="quantName">Имя кванта в системе</param>
        IQuant GetQuant(string quantName);

        /// <summary>
        /// Интерфейс инстанса провайдера котировок
        /// </summary>
        IL1QuotationProvider L1QuotationProvider { get; }

        /// <summary>
        /// Интерфейс инстанса хранилища котировок
        /// </summary>
        IL1QuotationStore L1QuotationStore { get; }

        /// <summary>
        /// Интерфейс инстанса торгового движка
        /// </summary>
        ITradingEngine TradingEngine { get; }
    }
}
