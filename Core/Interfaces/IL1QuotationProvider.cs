using QuantaBasket.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuantaBasket.Core.Interfaces
{
    public interface IL1QuotationProvider
    {
        void OnNewQuotations(Action<IEnumerable<L1Quotation>> processQuotations);
    }
}
