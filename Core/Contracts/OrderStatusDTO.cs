using QuantaBasket.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuantaBasket.Core.Contracts
{
    /// <summary>
    /// Информация о статусе рыночного ордера
    /// Предоставляет торговая система
    /// </summary>
    public sealed class OrderStatusDTO
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
        /// Рыночное время
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
        /// Текущий статус ордера
        /// В терминах статуса сигнала
        /// </summary>
        public SignalStatus Status { get; set; }

        /// <summary>
        /// Комментарии к статусу ордера от торговой системы
        /// </summary>
        public string Text { get; set; }
    }
}
