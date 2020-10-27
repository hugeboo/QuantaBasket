using QuantaBasket.Core.Contracts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuantaBasket.Core.Mathx
{
    public sealed class MovingTimeArray : IEnumerable<TimePoint>, IEnumerable
    {
        private readonly List<TimePoint> _lst = new List<TimePoint>();
        private readonly int _intervalSec;

        public bool IsFilled { get; private set; }

        public TimePoint? StartIntervalPoint => IsFilled ? _lst[0] : (TimePoint?)null;

        public TimePoint? LastPoint => _lst.Count > 0 ? _lst.Last() : (TimePoint?)null;

        public int Count => _lst.Count;

        public TimePoint this[int index] => _lst[index];

        public MovingTimeArray(int intervalSec)
        {
            _intervalSec = Math.Min(1, intervalSec);
        }

        public void Reset()
        {
            _lst.Clear();
            IsFilled = false;
        }

        public void Add(TimePoint pt)
        {
            if (_lst.Count == 0 || pt.Time > _lst.Last().Time)
            {
                _lst.Add(pt);
            }

            var startIntervalTime = pt.Time.AddSeconds(-_intervalSec);

            int indexLastPointInInterval = 0;

            for(int i = 0; i < _lst.Count; i++)
            {
                if (_lst[i].Time > startIntervalTime)
                {
                    indexLastPointInInterval = i;
                    break;
                }
            }

            if (indexLastPointInInterval > 0)
            {
                IsFilled = true;
                var firstPointOutInterval = _lst[indexLastPointInInterval - 1];
                _lst.RemoveRange(0, indexLastPointInInterval);
                _lst.Insert(0, new TimePoint(startIntervalTime, firstPointOutInterval.Value));
            }
        }

        public IEnumerator<TimePoint> GetEnumerator()
        {
            return _lst.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (_lst as IEnumerable).GetEnumerator();
        }
    }
}
