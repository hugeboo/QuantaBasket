using QuantaBasket.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuantaBasket.Core.Mathx
{
    public sealed class DoubleSMA : IStreamDataFunction<TimePoint, DoubleSMA.Result>
    {
        private readonly SMA _slowSMA;
        private readonly SMA _fastSMA;

        private Action<Result> _processOutputData;
        private TimePoint? _lastSlowPoint = null;
        private TimePoint? _lastFastPoint = null;
        private TimePoint? _prevLastSlowPoint = null;
        private TimePoint? _prevLastFastPoint = null;

        public DoubleSMA(int slowPeriod, int fastPeriod)
        {
            _slowSMA = new SMA(slowPeriod);
            _slowSMA.RegisterCallback(pt => { _prevLastSlowPoint = _lastSlowPoint; _lastSlowPoint = pt; });
            _fastSMA = new SMA(fastPeriod);
            _fastSMA.RegisterCallback(pt => { _prevLastFastPoint = _lastFastPoint; _lastFastPoint = pt; });
        }

        public void Add(TimePoint sourceData)
        {
            _slowSMA.Add(sourceData);
            _fastSMA.Add(sourceData);

            if (_lastSlowPoint.HasValue && _lastFastPoint.HasValue &&
                _prevLastSlowPoint.HasValue && _prevLastFastPoint.HasValue)
            {
                _processOutputData?.Invoke(new Result
                {
                    SlowSMA = _lastSlowPoint.Value,
                    FastSMA = _lastFastPoint.Value,
                    PrevSlowSMA = _prevLastSlowPoint.Value,
                    PrevFastSMA = _prevLastFastPoint.Value,
                });
             }
        }

        public void RegisterCallback(Action<Result> processOutputData)
        {
            _processOutputData = processOutputData;
        }

        public void Reset()
        {
            _slowSMA.Reset();
            _lastSlowPoint = null;
            _fastSMA.Reset();
            _lastFastPoint = null;
        }

        public struct Result
        {
            public TimePoint SlowSMA { get; set; }
            public TimePoint FastSMA { get; set; }
            public TimePoint PrevSlowSMA { get; set; }
            public TimePoint PrevFastSMA { get; set; }
            public decimal Delta => FastSMA.Value - SlowSMA.Value;
            public decimal PrevDelta => PrevFastSMA.Value - PrevSlowSMA.Value;
            public Event Event
            {
                get
                {
                    if ((Math.Sign(Delta) == 0 || Math.Sign(Delta) != Math.Sign(PrevDelta)) && Math.Sign(PrevDelta) != 0)
                    {
                        return Math.Sign(PrevDelta) > 0 ? Event.FastBreakAbove : Event.FastBreakBelow;
                    }
                    return Event.None;
                }
            }
        }

        public enum Event
        {
            None,
            FastBreakAbove, // быстрая пробила сверху
            FastBreakBelow // быстрая пробила снизу
        }
    }
}
