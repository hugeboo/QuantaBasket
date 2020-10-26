using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace QuantaBasket.Core.Utils
{
    public sealed class AsyncWorker<T> : IDisposable
    {
        private readonly Action<T> _itemAction;
        private readonly Action _idleAction;
        private readonly int _idlePeriod;
        private readonly object _syncObj = new object();
        private readonly List<T> _itemQueue = new List<T>();
        private readonly AutoResetEvent _newItemEvent = new AutoResetEvent(false);
        private readonly Thread _thread;
        private readonly string _name;
        private bool _bStopSignal = false;

        private ILogger _logger;

        public AsyncWorker(string name, Action<T> itemAction, Action idleAction = null, int idlePeriodMSec = 1000)
        {
            _itemAction = itemAction ?? throw new ArgumentNullException(nameof(itemAction));
            _idleAction = idleAction;
            _idlePeriod = System.Math.Min(10, idlePeriodMSec);
            _name = name;
            _logger = LogManager.GetLogger($"AsyncWorker:{_name}");

            _thread = new Thread(ThreadWorkerProc);
            _thread.Start();
        }

        public void AddItem(T item)
        {
            lock (_syncObj) _itemQueue.Add(item);
            _newItemEvent.Set();
        }

        public void AddItemRange(IEnumerable<T> items)
        {
            lock (_syncObj) _itemQueue.AddRange(items);
            _newItemEvent.Set();
        }

        public void Dispose()
        {
            _bStopSignal = true;
            if (!_thread.Join(2000)) _thread.Abort();
        }

        private void ThreadWorkerProc(object state)
        {
            try
            {
                while (!_bStopSignal)
                {
                    while (!_bStopSignal)
                    {
                        if (!_newItemEvent.WaitOne(1000)) _idleAction?.Invoke(); else break;
                    }
                    if (_bStopSignal) return;

                    T[] arr;
                    lock (_syncObj)
                    {
                        arr = _itemQueue.ToArray();
                        _itemQueue.Clear();
                    }

                    foreach (var it in arr)
                    {
                        try
                        {
                            _itemAction(it);
                        }
                        catch (Exception ex)
                        {
                            _logger.Error(ex, $"Process item failed. Item: {it} Error: {ex.Message}");
                        }
                    }
                }
            }
            catch (ThreadAbortException)
            {
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }
    }
}
