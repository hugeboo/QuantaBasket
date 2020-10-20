using QuantaBasket.Core.Contracts;
using QuantaBasket.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuantaBasket.SQLiteL1QuotationStore
{
    public sealed class SQLiteL1QuotationStore : IL1QuotationStore
    {
        public void Insert(IEnumerable<L1Quotation> quations)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<L1Quotation> Select(L1QuotationFilter filter)
        {
            throw new NotImplementedException();
        }
    }
}
