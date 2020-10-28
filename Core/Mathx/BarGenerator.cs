using NLog;
using QuantaBasket.Core.Contracts;
using QuantaBasket.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace QuantaBasket.Core.Mathx
{
    /// <summary>
    /// Генератор баров (свечей) в реальном времени
    /// Время берется из потока поступающих на вход котировок (т.е. рыночное)
    /// </summary>
    public sealed class BarGenerator : IStreamDataFunction<L1Quotation, OHLCV>
    {
        private readonly BarInterval _intervalSec;
        private readonly TimeSpan _intervalTs;
        private Action<OHLCV> _barProcessor;

        private OHLCV _currentBar;

        /// <summary>
        /// Конструктор генератора
        /// </summary>
        /// <param name="intervalSec">Ширина бара в секундах</param>
        public BarGenerator(BarInterval intervalSec)
        {
            _intervalSec = intervalSec;
            _intervalTs = TimeSpan.FromSeconds((int)_intervalSec);
        }

        public void RegisterCallback(Action<OHLCV> processOutputData)
        {
            _barProcessor = processOutputData;
        }

        /// <summary>
        /// Входной поток: цена/проторгованный объем/время
        /// </summary>
        /// <param name="pt">Цена</param>
        /// <param name="volume">Проторгованный объем</param>
        /// <param name="time">Время</param>
        public void Add(decimal pt, long volume, DateTime time)
        {
            Add(new L1Quotation 
            {
                DateTime = time,
                Last = pt,
                DVolume = volume
            });
        }

        /// <summary>
        /// Очистка внутреннего буфера
        /// </summary>
        public void Reset()
        {
            _currentBar = null;
        }

        /// <summary>
        /// Входной поток: котировки L1
        /// Бар строится по цене Last
        /// </summary>
        /// <param name="q">Котировка L1</param>
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
}
