using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuantaBasket.Core.Contracts
{
    public sealed class OHLCV : ICloneable
    {
        private static CultureInfo _culture = new CultureInfo("En-us");

        public DateTime StartTime { get; set; }
        public BarInterval IntervalSec { get; set; }
        public decimal Open { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public decimal Close { get; set; }
        public long Volume { get; set; }

        public OHLCV Clone2()
        {
            return Clone() as OHLCV;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public override string ToString()
        {
            return $"T:{StartTime:HH:mm:ss} O:{Open.ToString(_culture)} H:{High.ToString(_culture)} L:{Low.ToString(_culture)} C:{Close.ToString(_culture)} V:{Volume}";
        }
    }

    public enum BarInterval
    {
        Sec5 = 5, // for tests
        Min1 = 60,
        Min5 = 300,
        Min15 = 900,
        Min30 = 1800,
        Min60 = 3600
    }
}
