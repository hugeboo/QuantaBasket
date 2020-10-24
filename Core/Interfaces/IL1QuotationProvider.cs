using QuantaBasket.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuantaBasket.Core.Interfaces
{
    /// <summary>
    /// Провайдер котировок L1
    /// </summary>
    public interface IL1QuotationProvider : IErrorReporter, IDisposable
    {
        /// <summary>
        /// Зарегистрировать обработчик потока котировок
        /// </summary>
        /// <param name="processQuotations"></param>
        void RegisterQuotationProcessor(Action<IEnumerable<L1Quotation>> processQuotations);

        /// <summary>
        /// Подключиться к тороговой системе
        /// </summary>
        void Connect();

        /// <summary>
        /// Отключиться от торговой системы
        /// </summary>
        void Disconnect();
    }
}
