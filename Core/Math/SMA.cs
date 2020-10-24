using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuantaBasket.Core.Math
{
    /// <summary>
    /// Калькулятор скоьзящего среднего в режиме реального времени
    /// Simple Moving Average
    /// </summary>
    public sealed class SMA
    {
        private readonly Action<decimal> _smaProcessor;
        private readonly List<decimal> _buffer = new List<decimal>();
        private readonly int _period;

        private long _index = 0;
        private decimal _sum = 0m;

        public long TotalSourcePointCount => _index;

        /// <summary>
        /// Конструктор калькулятора
        /// </summary>
        /// <param name="period">Период СС</param>
        /// <param name="smaProcessor">Метод для обработки выходного потока данных СС</param>
        public SMA(int period, Action<decimal> smaProcessor)
        {
            _period = period;
            _smaProcessor = smaProcessor ?? throw new ArgumentNullException(nameof(smaProcessor));
        }

        /// <summary>
        /// Входной поток данных
        /// </summary>
        public void Add(decimal pt)
        { 
            if (_index < _period - 1)
            {
                _buffer.Add(pt);
                _sum += pt;
            }
            else if (_index == _period - 1)
            {
                _buffer.Add(pt);
                _sum += pt;
                var sma = _sum / _period;
                _smaProcessor(sma);
            }
            else
            {
                _sum -= _buffer[0];
                _buffer.RemoveAt(0);

                _buffer.Add(pt);
                _sum += pt;
                var sma = _sum / _period;
                _smaProcessor(sma);
            }

            _index++;
        }
    }
}
