using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuantaBasket.Core.Contracts
{
    /// <summary>
    /// Котировка L1 
    /// В QUIK это строка из "Текуще таблицы параметров".
    /// </summary>
    public sealed class L1Quotation : ICloneable
    {
        private readonly CultureInfo _culture = CultureInfo.GetCultureInfo("En-us");

        /// <summary>
        /// Идентификатор бумаги
        /// </summary>
        public SecurityId Security { get; set; }

        /// <summary>
        /// Время котировки, получаемое из торговой системы
        /// </summary>
        public DateTime DateTime { get; set; }

        /// <summary>
        /// Спрос
        /// </summary>
        public decimal Bid { get; set; }

        /// <summary>
        /// Предложение
        /// </summary>
        public decimal Ask { get; set; }

        /// <summary>
        /// Цена последней сделки
        /// </summary>
        public decimal Last { get; set; }

        public long LastSize { get; set; }

        /// <summary>
        /// Объем проторгованный за день
        /// </summary>
        public long Volume { get; set; }


        /// <summary>
        /// Изменение проторгованного объема относительно предыдущей котировки
        /// Фактически, это объем последней сделки
        /// </summary>
        public long DVolume { get; set; }

        /// <summary>
        /// Флаги изменения относительно предыдущей котировки
        /// </summary>
        public L1QuotationChangedFlags Changes { get; set; }

        /// <summary>
        /// Клонирование с приведением к типу
        /// </summary>
        public L1Quotation Clone2()
        {
            return Clone() as L1Quotation;
        }

        public object Clone()
        {
            var c = this.MemberwiseClone() as L1Quotation;
            c.Security = Security?.Clone2();
            return c;
        }

        public override string ToString()
        {
            return $"{Security} {DateTime:HH:mm:ss} B:{Bid.ToString(_culture)} A:{Ask.ToString(_culture)} L:{Last.ToString(_culture)} LS:{LastSize} V:{Volume} DV:{DVolume} Changes:{Changes}";
        }
    }

    /// <summary>
    /// Флаги изменения относительно предыдущей котировки
    /// Флаги соответствуют полям L1Quotation
    /// </summary>
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
        LastSize = 64,

        Trade = 65536,

        All = Bid | Ask | Last | Volume | Time | DVolume | LastSize
    }
}
