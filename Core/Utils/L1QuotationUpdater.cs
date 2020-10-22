using QuantaBasket.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuantaBasket.Core.Utils
{
    public static class L1QuotationUpdater
    {
        public static bool Update(L1Quotation oldQuote, L1Quotation quote)
        {
            if (oldQuote.DateTime > quote.DateTime) return false;

            var changes = L1QuotationChangedFlags.None;

            if (oldQuote.DateTime != quote.DateTime) { oldQuote.DateTime = quote.DateTime; changes |= L1QuotationChangedFlags.Time; }
            if (oldQuote.Bid != quote.Bid) { oldQuote.Bid = quote.Bid; changes |= L1QuotationChangedFlags.Bid; }
            if (oldQuote.Ask != quote.Ask) { oldQuote.Ask = quote.Ask; changes |= L1QuotationChangedFlags.Ask; }
            if (oldQuote.Last != quote.Last) { oldQuote.Last = quote.Last; changes |= L1QuotationChangedFlags.Last; }
            if (oldQuote.Volume != quote.Volume) { oldQuote.Volume = quote.Volume; changes |= L1QuotationChangedFlags.Volume; }

            if (changes != L1QuotationChangedFlags.None)
            {
                oldQuote.Changes = changes;
                return true;
            }

            return false;
        }
    }
}
