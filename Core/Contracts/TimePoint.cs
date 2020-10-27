using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuantaBasket.Core.Contracts
{
    public struct TimePoint
    {
        private static CultureInfo _culture = new CultureInfo("En-us");

        public DateTime Time { get; }
        public decimal Value { get; }
        public long Ticks => Time.Ticks;

        public TimePoint(DateTime time, decimal value)
        {
            Time = time;
            Value = value;
        }

        public override string ToString()
        {
            return string.Join(";", 
                Time.ToString("HH:mm:ss.fff"),
                Ticks,
                Value.ToString(_culture));
        }
    }
}
