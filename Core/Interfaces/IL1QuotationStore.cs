using QuantaBasket.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuantaBasket.Core.Interfaces
{
    /// <summary>
    /// Хранилище котировок L1 (база данных)
    /// </summary>
    public interface IL1QuotationStore
    {
        /// <summary>
        /// Сохранить котировки в БД
        /// </summary>
        void Insert(IEnumerable<L1Quotation> quations);

        /// <summary>
        /// Выбрать котировки по фильтру
        /// </summary>
        /// <param name="filter">Фильтр котировок</param>
        IEnumerable<L1Quotation> Select(L1QuotationFilter filter);
    }
}
