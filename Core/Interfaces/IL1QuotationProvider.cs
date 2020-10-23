using QuantaBasket.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuantaBasket.Core.Interfaces
{
    public interface IL1QuotationProvider : IErrorReporter, IDisposable
    {
        void RegisterQuotationProcessor(Action<IEnumerable<L1Quotation>> processQuotations);
        void Connect();
        void Disconnect();
    }
}
