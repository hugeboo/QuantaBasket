using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuantaBasket.Core.Interfaces
{
    /// <summary>
    /// Интерфейс сигнала для передачи в торговую систему
    /// </summary>
    public interface ITradeSignal
    {
        /// <summary>
        /// Уникальный идентификатор
        /// </summary>
        string Id { get; }

        /// <summary>
        /// Код класса бумаги
        /// </summary>
        string ClassCode { get; }

        /// <summary>
        /// Код бумагт (тикер)
        /// </summary>
        string SecCode { get; }

        /// <summary>
        /// Сторона сделки (купля/продажа)
        /// </summary>
        SignalSide Side { get; }

        /// <summary>
        /// Объем сделки в штуках
        /// </summary>
        long Qtty { get; }

        /// <summary>
        /// Цена (не актуальна для сигналов типа Market)
        /// </summary>
        decimal Price { get; }

        /// <summary>
        /// Тип цены сделки (Market, Limit, Stop)
        /// </summary>
        PriceType PriceType { get; }
    }
}
