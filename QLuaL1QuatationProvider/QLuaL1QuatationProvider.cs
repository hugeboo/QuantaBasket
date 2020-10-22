using QuantaBasket.Core.Contracts;
using QuantaBasket.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NLog;
using QuantaBasket.QLuaL1QuatationProvider.Properties;
using System.Net.Sockets;
using System.Threading;

namespace QuantaBasket.QLuaQuatationProvider
{
    public sealed class QLuaL1QuatationProvider : IL1QuotationProvider, IDisposable
    {
        private readonly IL1QuotationStore _store;
        private Action<ErrorReportCode, string> _onErrorAction;
        private Action<IEnumerable<L1Quotation>> _onNewQuotationsAction;
        private Socket _socket;
        private Thread _thread;
        private ILogger _logger = LogManager.GetCurrentClassLogger();

        public QLuaL1QuatationProvider(IL1QuotationStore store)
        {
            _store = store ?? throw new ArgumentNullException(nameof(store));
        }

        public void OnError(Action<ErrorReportCode, string> processError)
        {
            _onErrorAction = processError;
            _logger.Debug(processError != null ? 
                "Error processor assigned" : 
                "Error processor unassigned");
        }

        public void OnNewQuotations(Action<IEnumerable<L1Quotation>> processQuotations)
        {
            _onNewQuotationsAction = processQuotations;
            _logger.Debug(processQuotations != null ? 
                "Quotations processor assigned" : 
                "Quotations processor unassigned");
        }

        public void Dispose()
        {
            _logger.Debug("Disposing");
            try
            {
                Disconnect();
            }
            catch { }
        }

        public void Connect()
        {
            var addr = Settings.Default.QLuaAddr;
            var port = Settings.Default.QLuaPort;

            _logger.Debug($"Connectiong to {addr}:{port}");

            _socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
            _socket.ReceiveBufferSize = 100 * 1024;
            _socket.Connect(addr, port);

            _logger.Info($"Connected to {addr}:{port}");

            _thread = new Thread(ProcessQuotesThreadProc);
            _thread.Start();

            var requestQuotes = Settings.Default.Securities;

            _logger.Debug($"Requesting quotes {requestQuotes}");

            _socket.Send(Encoding.UTF8.GetBytes(requestQuotes+"\r\n"));

            _logger.Info($"Quotest requested {requestQuotes}");
        }

        public void Disconnect()
        {
            _logger.Debug("Disconnecting");

            _thread?.Abort();
            _socket?.Dispose();

            _logger.Info("Disconnected");
        }

        private void ProcessQuotesThreadProc(object state)
        {
            try
            {
                _logger.Debug("ProcessQuotesThread started");

                //...
            }
            catch (ThreadAbortException)
            {
                _logger.Debug("ProcessQuotesThread aborted");
            }
            catch(Exception ex)
            {
                //...
            }
        }
    }
}
