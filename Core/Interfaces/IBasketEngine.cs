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
        /// Интерфейс провайдера котировок
        /// </summary>
        IL1QuotationProvider L1QuotationProvider { get; }
    }
}
