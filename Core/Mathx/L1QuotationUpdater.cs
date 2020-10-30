using QuantaBasket.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuantaBasket.Core.Mathx
{
    /// <summary>
    /// "Обновлятор" котирвок L1
    /// </summary>
    public static class L1QuotationUpdater
    {
        /// <summary>
        /// Обновить старую котировку более новой и выставить соответствующие флаги изменений
        /// </summary>
        /// <param name="quote">Старая котировка</param>
        /// <param name="newQuote">Новая котировка</param>
        /// <returns>Наличие изменений</returns>
        public static bool Update(L1Quotation quote, L1Quotation newQuote)
        {
            //if (quote.DateTime > newQuote.DateTime) return false;

            var changes = L1QuotationChangedFlags.None;

            if (quote.DateTime != newQuote.DateTime) { quote.DateTime = newQuote.DateTime; changes |= L1QuotationChangedFlags.Time; }

            if (newQuote.Bid != 0m && quote.Bid != newQuote.Bid) { quote.Bid = newQuote.Bid; changes |= L1QuotationChangedFlags.Bid; }
            if (newQuote.Ask != 0m && quote.Ask != newQuote.Ask) { quote.Ask = newQuote.Ask; changes |= L1QuotationChangedFlags.Ask; }

            if (newQuote.Last != 0m || newQuote.LastSize != 0) 
            { 
                quote.Last = newQuote.Last;
                quote.LastSize = newQuote.LastSize;
                changes |= L1QuotationChangedFlags.Trade;
                if (quote.Last != newQuote.Last) { quote.Last = newQuote.Last; changes |= L1QuotationChangedFlags.Last; }
                if (quote.LastSize != newQuote.LastSize) { quote.LastSize = newQuote.LastSize; changes |= L1QuotationChangedFlags.LastSize; }
            }

            if (newQuote.Volume != 0)
            {
                var dVolume = newQuote.Volume - quote.Volume;
                if (quote.DVolume != dVolume) { quote.DVolume = dVolume; changes |= L1QuotationChangedFlags.DVolume; }
                if (quote.Volume != newQuote.Volume) { quote.Volume = newQuote.Volume; changes |= L1QuotationChangedFlags.Volume; }
            }

            if (changes != L1QuotationChangedFlags.None)
            {
                quote.Changes = changes;
                return true;
            }

            return false;
        }
    }
}
