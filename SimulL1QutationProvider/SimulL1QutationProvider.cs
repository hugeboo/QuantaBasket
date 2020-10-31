using NLog;
using QuantaBasket.Core.Contracts;
using QuantaBasket.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace QuantaBasket.Components.SimulL1QuotationProvider
{
    public sealed class SimulL1QutationProvider : IL1QuotationProvider
    {
        private readonly IL1QuotationStore _store;
        private Action<ErrorReportCode, string> _onErrorAction;
        private Action<IEnumerable<L1Quotation>> _onNewQuotationsAction;
        private ILogger _logger = LogManager.GetLogger("SimulL1QutationProvider");
        private CancellationTokenSource _cancelSource;
        private Task _simulatorTask;

        public bool Connected { get; private set; }

        public SimulL1QutationProvider(IL1QuotationStore store)
        {
            _store = store;
        }

        public void Connect()
        {
            Connected = true;
            _cancelSource = new CancellationTokenSource();
            _simulatorTask = Task.Factory.StartNew(SimulatorSimpleProc, _cancelSource.Token);
        }

        public void Disconnect()
        {
            Connected = false;
            _cancelSource?.Cancel();
        }

        public void Dispose()
        {
            try
            {
                _cancelSource?.Cancel();
                _simulatorTask?.Dispose();
                _cancelSource?.Dispose();
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }

        public void RegisterCallback(Action<IEnumerable<L1Quotation>> processQuotations)
        {
            _onNewQuotationsAction = processQuotations;
        }

        public void RegisterCallback(Action<ErrorReportCode, string> processError)
        {
            _onErrorAction = processError;
        }

        public object GetConfiguration()
        {
            return Configuration.Instance;
        }

        public void SaveConfiguration()
        {
            Configuration.Instance.Save();
        }

        private void SimulatorSimpleProc()
        {
            try
            {
                Thread.Sleep(1000);
                var totalCount = _store.SelectCount();
                var offset = 0;
                while (offset < totalCount && !_cancelSource.Token.IsCancellationRequested)
                {
                    var quotes = _store.SelectPage(100, offset);
                    offset += 100;
                    _onNewQuotationsAction(quotes);
                    Thread.Sleep(50);
                }
            }
            catch(Exception ex)
            {
                _logger.Error(ex);
                _onErrorAction?.Invoke(ErrorReportCode.Unknown, ex.Message);
            }
        }
    }
}
