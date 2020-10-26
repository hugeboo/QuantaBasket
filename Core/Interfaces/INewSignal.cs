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
        /// Дата/время создания сигнала
        /// </summary>
        DateTime CreatedTime { get; }

        /// <summary>
        /// Код класса бумаги
        /// </summary>
        string ClassCode { get; set; }

        /// <summary>
        /// Код бумагт (тикер)
        /// </summary>
        string SecCode { get; set; }

        /// <summary>
        /// Текущий статус сигнала
        /// </summary>
        SignalStatus Status { get; }

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

        /// <summary>
        /// Исполненная часть сигнала
        /// </summary>
        long ExecQtty { get; }

        /// <summary>
        /// Средняя цена исполнения
        /// </summary>
        decimal AvgPrice { get; }
    }
}
