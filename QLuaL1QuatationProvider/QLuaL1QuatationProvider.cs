using QuantaBasket.Core.Contracts;
using QuantaBasket.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuantaBasket.QLuaQuatationProvider
{
    public sealed class QLuaQuatationProvider : IL1QuotationProvider
    {
        public void OnNewQuotations(Action<IEnumerable<L1Quotation>> processQuotations)
        {
            throw new NotImplementedException();
        }
    }
}
