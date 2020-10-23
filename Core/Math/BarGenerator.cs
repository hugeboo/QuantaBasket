using NLog;
using QuantaBasket.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace QuantaBasket.Core.Math
{
    public sealed class BarGenerator
    {
        private readonly BarInterval _intervalSec;
        private readonly TimeSpan _intervalTs;
        private readonly Action<OHLCV> _barProcessor;

        private OHLCV _currentBar;

        public BarGenerator(BarInterval intervalSec, Action<OHLCV> barProcessor)
        {
            _intervalSec = intervalSec;
            _intervalTs = TimeSpan.FromSeconds((int)_intervalSec);
            _barProcessor = barProcessor ?? throw new ArgumentNullException(nameof(barProcessor));
        }

        public void Add(decimal pt, long volume, DateTime time)
        {
            Add(new L1Quotation 
            {
                DateTime = time,
                Last = pt,
                DVolume = volume
            });
        }

        public void Add(L1Quotation q)
        {
            if (_currentBar == null)
            {
                var currentSec = q.DateTime.TimeOfDay.TotalSeconds;
                var startSec = (int)(System.Math.Floor(currentSec / (int)_intervalSec) * (int)_intervalSec);
                _currentBar = new OHLCV
                {
                    IntervalSec = _intervalSec,
                    StartTime = q.DateTime.Date + TimeSpan.FromSeconds(startSec),
                    Open = q.Last,
                    High = q.Last,
                    Low = q.Last,
                    Close = q.Last,
                    Volume = q.DVolume
                };
            }
            else if (q.DateTime<_currentBar.StartTime + _intervalTs)
            {
                _currentBar.Volume += q.DVolume;
                if (q.Last > _currentBar.High) _currentBar.High = q.Last;
                if (q.Last < _currentBar.Low) _currentBar.Low = q.Last;
                _currentBar.Close = q.Last;
            }
            else
            {
                _barProcessor(_currentBar);

                var nextStartTime = _currentBar.StartTime + _intervalTs;

                while(nextStartTime < q.DateTime)
                {
                    var emptyBar = new OHLCV 
                    {
                        IntervalSec = _intervalSec,
                        StartTime = nextStartTime,
                        Open = _currentBar.Close,
                        High = _currentBar.Close,
                        Low = _currentBar.Close,
                        Close = _currentBar.Close,
                        Volume = 0
                    };
                    _barProcessor(emptyBar);
                    nextStartTime += _intervalTs;
                }

                _currentBar = new OHLCV
                {
                    IntervalSec = _intervalSec,
                    StartTime = nextStartTime - _intervalTs,
                    Open = q.Last,
                    High = q.Last,
                    Low = q.Last,
                    Close = q.Last,
                    Volume = q.DVolume
                };
            }
        }
    }

    //    public sealed class RealTimeBarGenerator : IDisposable
    //{
    //    private readonly BarInterval _intervalsec;
    //    private readonly Action<OHLCV> _barProcessor;

    //    private Thread _thread;
    //    private readonly List<L1Quotation> _lstQuotes = new List<L1Quotation>();

    //    private ILogger _logger = LogManager.GetLogger("RealTimeBarGenerator");

    //    public RealTimeBarGenerator(BarInterval intervalSec, Action<OHLCV> barProcessor)
    //    {
    //        _intervalsec = intervalSec;
    //        _barProcessor = barProcessor ?? throw new ArgumentNullException(nameof(barProcessor));

    //        var currentSec = DateTime.Now.TimeOfDay.TotalSeconds;
    //        var startSec = (int)(System.Math.Floor(currentSec / (int)intervalSec) * (int)intervalSec);
    //        var currentBar = new OHLCV 
    //        { 
    //            IntervalSec = _intervalsec, 
    //            StartTime = DateTime.Today + TimeSpan.FromSeconds(startSec),
    //            Low = decimal.MaxValue
    //        };

    //        _thread = new Thread(ThreadProc);
    //        _thread.Start(currentBar);
    //    }

    //    public void AddQuotation(L1Quotation q)
    //    {
    //        lock (_lstQuotes) _lstQuotes.Add(q);
    //    }

    //    private void ThreadProc(object state)
    //    {
    //        try
    //        {
    //            var currentBar = state as OHLCV;
    //            while (true)
    //            {
    //                if (DateTime.Now.TimeOfDay.TotalSeconds - currentBar.StartTime.TimeOfDay.TotalSeconds >= (int)_intervalsec)
    //                {
    //                    var nextStartTime = currentBar.StartTime + TimeSpan.FromSeconds((int)_intervalsec);
    //                    bool openSet = false;
    //                    decimal? close = null;
    //                    OHLCV readyBar;
    //                    lock (_lstQuotes)
    //                    {
    //                        for (int i = 0; i < _lstQuotes.Count; i++)
    //                        {
    //                            var q = _lstQuotes[0];
    //                            Console.WriteLine(q);
    //                            if (q.DateTime < currentBar.StartTime)
    //                            {
    //                                _lstQuotes.RemoveAt(0);
    //                                i--;
    //                            }
    //                            else if (q.DateTime < nextStartTime)
    //                            {
    //                                currentBar.Volume += q.DVolume;
    //                                if (q.Last > currentBar.High) currentBar.High = q.Last;
    //                                if (q.Last < currentBar.Low) currentBar.Low = q.Last;
    //                                if (!openSet) { currentBar.Open = q.Last; openSet = true; }
    //                                close = q.Last;
    //                                _lstQuotes.RemoveAt(0);
    //                                i--;
    //                            }
    //                        }

    //                        if (close.HasValue) currentBar.Close = close.Value;
    //                        if (currentBar.Low == decimal.MaxValue) currentBar.Low = 0;

    //                        readyBar = currentBar;
    //                        currentBar = new OHLCV 
    //                        { 
    //                            IntervalSec = _intervalsec, 
    //                            StartTime = nextStartTime,
    //                            Low = decimal.MaxValue
    //                        };
    //                    }
    //                    _barProcessor(readyBar);
    //                }
    //                Thread.Sleep(10);
    //            }
    //        }
    //        catch (ThreadAbortException) 
    //        { 
    //        }
    //        catch(Exception ex)
    //        {
    //            _logger.Error(ex);
    //        }
    //    }

    //    public void Dispose()
    //    {
    //        if (!_thread.Join(1000)) _thread.Abort();
    //    }
    //}
}
