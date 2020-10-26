﻿using QuantaBasket.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuantaBasket.Core.Contracts
{
    /// <summary>
    /// Информации о рыночной сделке (трейде) в рамках сигнала
    /// Предоставляет торговая система
    /// </summary>
    public sealed class TradeDTO
    {
        /// <summary>
        /// Уникальный идентификатор сигнала
        /// </summary>
        public string SignalId { get; set; }

        /// <summary>
        /// Рыночный идентификатор ордера
        /// </summary>
        public string MarketOrderId { get; set; }

        /// <summary>
        /// Рыночный идентификатор трейда
        /// </summary>
        public string MarketTradeId { get; set; }

        /// <summary>
        /// Рыночное время трейда
        /// </summary>
        public DateTime MarketDateTime { get; set; }

        /// <summary>
        /// Код класса бумаги
        /// </summary>
        public string ClassCode { get; set; }

        /// <summary>
        /// Код бумагт (тикер)
        /// </summary>
        public string SecCode { get; set; }

        /// <summary>
        /// Сторона сделки (купля/продажа)
        /// </summary>
        public SignalSide Side { get; set; }

        /// <summary>
        /// Размер сделки в штуках
        /// </summary>
        public long Qtty { get; set; }

        /// <summary>
        /// Цена 
        /// </summary>
        public decimal Price { get; set; }
    }
}
