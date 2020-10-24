using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuantaBasket.Core.Contracts
{
    /// <summary>
    /// Бар (свеча, candle)
    /// </summary>
    public sealed class OHLCV : ICloneable
    {
        private static CultureInfo _culture = new CultureInfo("En-us");

        /// <summary>
        /// Начальное время бара
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// Длительность бара (в секундах)
        /// </summary>
        public BarInterval IntervalSec { get; set; }

        /// <summary>
        /// Цена открытия
        /// </summary>
        public decimal Open { get; set; }

        /// <summary>
        /// Максимальная цена
        /// </summary>
        public decimal High { get; set; }

        /// <summary>
        /// Минимальная цена
        /// </summary>
        public decimal Low { get; set; }

        /// <summary>
        /// Цена закрытия
        /// </summary>
        public decimal Close { get; set; }

        /// <summary>
        /// Проторгованный объем в штуках
        /// </summary>
        public long Volume { get; set; }

        /// <summary>
        /// Клонирование с приведением типа
        /// </summary>
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
