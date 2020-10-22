using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuantaBasket.Core.Contracts
{
    public sealed class L1Quotation :ICloneable
    {
        private readonly CultureInfo _culture = CultureInfo.GetCultureInfo("En-us");

        public SecurityId Security { get; set; }
        public DateTime DateTime { get; set; }
        public decimal Bid { get; set; }
        public decimal Ask { get; set; }
        public decimal Last { get; set; }
        public long Volume { get; set; }
        public long DVolume { get; set; }
        public L1QuotationChangedFlags Changes { get; set; }
        public L1Quotation Clone2()
        {
            return Clone() as L1Quotation;
        }
        public object Clone()
        {
            var c = this.MemberwiseClone() as L1Quotation;
            c.Security = Security.Clone2();
            return c;
        }
        public override string ToString()
        {
            return $"{Security} {DateTime:HH:mm:ss} B:{Bid.ToString(_culture)} A:{Ask.ToString(_culture)} L:{Last.ToString(_culture)} V:{Volume} DV:{DVolume} Changes:{Changes}";
        }
    }

    [Flags]
    public enum L1QuotationChangedFlags
    {
        None = 0,
        Bid = 1,
        Ask = 2,
        Last = 4,
        Volume = 8,
        Time = 16,
        DVolume = 32,

        All = Bid | Ask | Last | Volume | Time | DVolume
    }
}
