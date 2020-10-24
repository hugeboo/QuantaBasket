using QuantaBasket.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuantaBasket.Core.Interfaces
{
    public interface IQuantSignal
    {
        string Id { get; }
        DateTime CreatedTime { get; }
        string ClassCode { get; set; }
        string SecCode { get; set; }
        SignalSide Side { get; set; }
        long Qtty { get; set; }
        decimal Price { get; set; }
        PriceType PriceType { get; set; }
        long ExecQtty { get; }
        decimal AvgPrice { get; }
    }

    public enum PriceType
    {
        Market,
        Limit,
        Stop
    }

    public enum SignalSide
    {
        Buy,
        Sell
    }

    public enum SignalStatus
    {
        New,
        Sent,
        Partial,
        Completed,
        Canceled,
        Rejected
    }
}
