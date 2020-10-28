using QuantaBasket.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuantaBasket.Core.Mathx
{
    /// <summary>
    /// Калькулятор скоьзящего среднего в режиме реального времени
    /// Simple Moving Average
    /// </summary>
    public sealed class SMA : IStreamDataFunction<TimePoint, TimePoint>
    {
        private Action<TimePoint> _smaProcessor;
        private readonly List<TimePoint> _buffer = new List<TimePoint>();
        private readonly int _period;

        private long _index = 0;
        private decimal _sum = 0m;

        public long TotalSourcePointCount => _index;

        public TimePoint FirstPoint => _buffer.FirstOrDefault();

        /// <summary>
        /// Конструктор калькулятора
        /// </summary>
        /// <param name="period">Период СС</param>
        /// <param name="smaProcessor">Метод для обработки выходного потока данных СС</param>
        public SMA(int period)
        {
            _period = period;
        }

        public void RegisterCallback(Action<TimePoint> processOutputData)
        {
            _smaProcessor = processOutputData;
        }

        /// <summary>
        /// Очистка внутреннего буфера
        /// </summary>
        public void Reset()
        {
            _buffer.Clear();
            _index = 0;
            _sum = 0m;
        }

        /// <summary>
        /// Входной поток данных
        /// </summary>
        public void Add(TimePoint pt)
        { 
            if (_index < _period - 1)
            {
                _buffer.Add(pt);
                _sum += pt.Value;
            }
            else if (_index == _period - 1)
            {
                _buffer.Add(pt);
                _sum += pt.Value;
                var sma = new TimePoint(pt.Time, _sum / _period);
                _smaProcessor(sma);
            }
            else
            {
                _sum -= _buffer[0].Value;
                _buffer.RemoveAt(0);

                _buffer.Add(pt);
                _sum += pt.Value;
                var sma = _sum / _period;
                _smaProcessor(new TimePoint(pt.Time, sma));
            }

            _index++;
        }
    }
}
