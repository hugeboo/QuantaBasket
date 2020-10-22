﻿using QuantaBasket.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuantaBasket.Core.Utils
{
    public static class L1QuotationUpdater
    {
        public static bool Update(L1Quotation quote, L1Quotation newQuote)
        {
            if (quote.DateTime > newQuote.DateTime) return false;

            var changes = L1QuotationChangedFlags.None;

            if (quote.DateTime != newQuote.DateTime) { quote.DateTime = newQuote.DateTime; changes |= L1QuotationChangedFlags.Time; }
            if (quote.Bid != newQuote.Bid) { quote.Bid = newQuote.Bid; changes |= L1QuotationChangedFlags.Bid; }
            if (quote.Ask != newQuote.Ask) { quote.Ask = newQuote.Ask; changes |= L1QuotationChangedFlags.Ask; }
            if (quote.Last != newQuote.Last) { quote.Last = newQuote.Last; changes |= L1QuotationChangedFlags.Last; }

            var dVolume = newQuote.Volume - quote.Volume;
            if (quote.DVolume != dVolume) { quote.DVolume = dVolume; changes |= L1QuotationChangedFlags.DVolume; }

            if (quote.Volume != newQuote.Volume) { quote.Volume = newQuote.Volume; changes |= L1QuotationChangedFlags.Volume; }

            if (changes != L1QuotationChangedFlags.None)
            {
                quote.Changes = changes;
                return true;
            }

            return false;
        }
    }
}
