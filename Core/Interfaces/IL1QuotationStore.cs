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
    public interface IL1QuotationStore : IHaveConfiguration
    {
        /// <summary>
        /// Сохранить котировки в БД
        /// </summary>
        void Insert(IEnumerable<L1Quotation> quations);

        int SelectCount();

        IEnumerable<L1Quotation> SelectPage(int limit, int offset);
    }
}
