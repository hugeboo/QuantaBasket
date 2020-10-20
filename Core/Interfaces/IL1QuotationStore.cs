using QuantaBasket.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuantaBasket.Core.Interfaces
{
    public interface IL1QuotationStore
    {
        void Insert(IEnumerable<L1Quotation> quations);
        IEnumerable<L1Quotation> Select(L1QuotationFilter filter);
    }
}
