using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuantaBasket.Core.Interfaces
{
    /// <summary>
    /// Торговый сигнал
    /// </summary>
    public interface ISignal
    {
        /// <summary>
        /// Средняя цена исполнения
        /// </summary>
        decimal AvgPrice { get; }

        /// <summary>
        /// Код класса бумаги
        /// </summary>
        string ClassCode { get; }

        /// <summary>
        /// Дата/время создания сигнала
        /// </summary>
        DateTime CreatedTime { get; }

        /// <summary>
        /// Исполненная часть сигнала
        /// </summary>
        long ExecQtty { get; }

        /// <summary>
        /// Уникальный идентификатор
        /// </summary>
        string Id { get; }

        /// <summary>
        /// Время последнего обновления 
        /// </summary>
        DateTime LastUpdateTime { get; }

        /// <summary>
        /// Рыночный идентификатор ордера
        /// </summary>
        string MarketOrderId { get; }

        /// <summary>
        /// Цена (не актуальна для сигналов типа Market)
        /// </summary>
        decimal Price { get; }

        /// <summary>
        /// Тип цены сделки (Market, Limit, Stop)
        /// </summary>
        PriceType PriceType { get; }

        /// <summary>
        /// Объем сделки в штуках
        /// </summary>
        long Qtty { get; }

        /// <summary>
        /// Имя кванта-создателя сигнала
        /// </summary>
        string QuantName { get; }

        /// <summary>
        /// Код бумагт (тикер)
        /// </summary>
        string SecCode { get; }

        /// <summary>
        /// Сторона сделки (купля/продажа)
        /// </summary>
        SignalSide Side { get; }

        /// <summary>
        /// Текущий статус сигнала
        /// </summary>
        SignalStatus Status { get; }
    }
}
