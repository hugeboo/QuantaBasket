using QuantaBasket.Core.Contracts;
using QuantaBasket.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuantaBasket.QLuaQuatationProvider
{
    public sealed class QLuaL1QuatationProvider : IL1QuotationProvider
    {
        private readonly IL1QuotationStore _store;

        public QLuaL1QuatationProvider(IL1QuotationStore store)
        {
            _store = store ?? throw new ArgumentNullException(nameof(store));
        }

        public void OnNewQuotations(Action<IEnumerable<L1Quotation>> processQuotations)
        {
            throw new NotImplementedException();
        }
    }
}
