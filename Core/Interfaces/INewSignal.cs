using QuantaBasket.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuantaBasket.Core.Interfaces
{
    /// <summary>
    /// Интерфейс вновь созданного сигнала
    /// </summary>
    public interface INewSignal
    {
        /// <summary>
        /// Уникальный идентификатор
        /// </summary>
        string Id { get; }

        /// <summary>
        /// Код класса бумаги
        /// </summary>
        string ClassCode { get; set; }

        /// <summary>
        /// Код бумагт (тикер)
        /// </summary>
        string SecCode { get; set; }

        /// <summary>
        /// Сторона сделки (купля/продажа)
        /// </summary>
        SignalSide Side { get; set; }

        /// <summary>
        /// Объем сделки в штуках
        /// </summary>
        long Qtty { get; set; }

        /// <summary>
        /// Цена (не актуальна для сигналов типа Market)
        /// </summary>
        decimal Price { get; set; }

        /// <summary>
        /// Тип цены сделки (Market, Limit, Stop)
        /// </summary>
        PriceType PriceType { get; set; }
    }
}
