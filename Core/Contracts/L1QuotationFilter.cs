using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuantaBasket.Core.Contracts
{
    /// <summary>
    /// Объект с параметрами поиска/фильтрации котировок в БД
    /// </summary>
    public sealed class L1QuotationFilter
    {
        /// <summary>
        /// Идентификатор бумаги
        /// </summary>
        public SecurityId Security { get; set; }

        /// <summary>
        /// Начальная дата/время включительно
        /// (необязательный параметр)
        /// </summary>
        public DateTime? DateTimeFrom { get; set; }

        /// <summary>
        /// Конечная дата/время включительно
        /// (необязательный параметр)
        /// </summary>
        public DateTime? DateTimeTo { get; set; }

        public override string ToString()
        {
            return $"{Security} {DateTimeFrom} {DateTimeTo}";
        }
    }
}
