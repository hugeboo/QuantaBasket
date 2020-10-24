using QuantaBasket.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuantaBasket.Core.Contracts
{
    /// <summary>
    /// Торговый сигнал
    /// Объект, содержащий исключительно данные
    /// </summary>
    public sealed class SignalDTO : IQuantSignal
    {
        public string Id { get; set; }
        public DateTime CreatedTime { get; set; }
        public string ClassCode { get; set; }
        public string SecCode { get; set; }
        public SignalStatus Status { get; set; }
        public SignalSide Side { get; set; }
        public long Qtty { get; set; }
        public decimal Price { get; set; }
        public PriceType PriceType { get; set; }
        public long ExecQtty { get; set; }
        public decimal AvgPrice { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public string QuantName { get; set; }

        public override string ToString()
        {
            return $"{Id} {ClassCode} {SecCode} {Side} {Qtty}@{Price} {PriceType}";
        }
    }
}
