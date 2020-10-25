using QuantaBasket.Core.Contracts;
using QuantaBasket.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NLog;
using System.Net.Sockets;
using System.Threading;
using QuantaBasket.Core.Exceptions;
using Newtonsoft.Json;
using QuantaBasket.Core.Math;
using System.Globalization;

namespace QuantaBasket.Components.QLuaL1QuotationProvider
{
    public sealed class QLuaL1QuotationProvider : IL1QuotationProvider, IHaveConfiguration
    {
        private readonly IL1QuotationStore _store;
        private readonly Dictionary<SecurityId, L1Quotation> _dictQuotes = new Dictionary<SecurityId, L1Quotation>();
        private readonly CultureInfo _culture = CultureInfo.GetCultureInfo("En-us");

        private Action<ErrorReportCode, string> _onErrorAction;
        private Action<IEnumerable<L1Quotation>> _onNewQuotationsAction;
        private Socket _socket;
        private Thread _thread;
        private ILogger _logger = LogManager.GetLogger("QLuaL1QuotationProvider");

        public bool Connected => (_socket?.Connected ?? false) && (_thread?.IsAlive ?? false);

        public QLuaL1QuotationProvider(IL1QuotationStore store)
        {
            _store = store;
        }

        public void RegisterErrorProcessor(Action<ErrorReportCode, string> processError)
        {
            _onErrorAction = processError;
            _logger.Debug(processError != null ? 
                "Error processor assigned" : 
                "Error processor unassigned");
        }

        public void RegisterQuotationProcessor(Action<IEnumerable<L1Quotation>> processQuotations)
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
            var addr = Configuration.Default.QLuaAddr;//Settings.Default.QLuaAddr;
            var port = Configuration.Default.QLuaPort;//Settings.Default.QLuaPort;

            _logger.Debug($"Connecting to {addr}:{port}");

            _socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
            _socket.ReceiveBufferSize = 100 * 1024;
            _socket.Connect(addr, port);

            _logger.Info($"Connected to {addr}:{port}");

            _thread = new Thread(ProcessQuotesThreadProc);
            _thread.Start();

            var requestQuotes = Configuration.Default.Securities;//Settings.Default.Securities;

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
            string m = string.Empty;
            try
            {
                _logger.Debug("ProcessQuotesThread started");

                while (true)
                {
                    dynamic d;
                    m = string.Empty;
                    L1Quotation q;

                    try
                    {
                        m = ReceiverJsonString.Receive(_logger, (b) => _socket.Receive(b));

                        d = JsonConvert.DeserializeAnonymousType(m, new 
                        {
                            @class = "", sec = "", last = "", bid = "", ask = "", voltoday = "", time = ""
                        });

                        q = new L1Quotation()
                        {
                            Security = new SecurityId { ClassCode = d.@class, SecurityCode = d.sec },
                            DateTime = DateTime.Parse(d.time),
                            Bid = decimal.Parse(d.bid, _culture),
                            Ask = decimal.Parse(d.ask, _culture),
                            Last = decimal.Parse(d.last, _culture),
                            Volume = (long)decimal.Parse(d.voltoday, _culture),
                            Changes = L1QuotationChangedFlags.None
                        };
                    }
                    catch(Exception ex)
                    {
                        throw new BadMessageException($"Message: {m}", ex);
                    }

                    ProcessL1Quotation(q);
                }
            }
            catch (ThreadAbortException)
            {
                _logger.Debug("ProcessQuotesThread aborted");
            }
            catch(TransportException ex)
            {
                _logger.Error(ex);
                _onErrorAction?.Invoke(ErrorReportCode.TransportError, ex.Message);
                Disconnect();
            }
            catch(BadMessageException ex)
            {
                _logger.Error(ex, $"Message: {m}");
                _onErrorAction?.Invoke(ErrorReportCode.TransportError, ex.Message);
                Disconnect();
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                _onErrorAction?.Invoke(ErrorReportCode.Unknown, ex.Message);
                Disconnect();
            }
        }

        private string ReceiveJsonString()
        {
            byte[] buffer = new byte[2048];
            int i = 0;
            bool inMessage = false;
            _socket.ReceiveTimeout = 0;

            while (true)
            {
                var b = new byte[1];

                try
                {
                    _socket.Receive(b);
                }
                catch(Exception ex)
                {
                    throw new TransportException(ex.Message, ex);
                }

                if (b[0] == 0) throw new ConnectionLostException("Received '0'", null);

                try
                {
                    if (!inMessage && b[0] == 123) // '{'
                    {
                        i = 0;
                        inMessage = true;
                        buffer[i++] = b[0];
                        _socket.ReceiveTimeout = 100;
                    }
                    else if (inMessage && b[0] == 125) // '}'
                    {
                        buffer[i++] = b[0];
                        var m = Encoding.UTF8.GetString(buffer, 0, i);
                        _logger.Trace($"Received: {m}");
                        return m;
                    }
                    else if (inMessage)
                    {
                        buffer[i++] = b[0];
                    }
                } 
                catch(Exception ex)
                {
                    throw new BadMessageException(ex.Message, ex);
                }
            }
        }

        private void ProcessL1Quotation(L1Quotation newQuote)
        {
            L1Quotation updatedQuote = null;

            if (!_dictQuotes.TryGetValue(newQuote.Security, out L1Quotation quote))
            {
                quote = newQuote;
                quote.Changes = L1QuotationChangedFlags.All;
                _dictQuotes[quote.Security] = quote;
                updatedQuote = quote.Clone2();
            }
            else
            {
                if (L1QuotationUpdater.Update(quote, newQuote)) updatedQuote = quote.Clone2();
            }

            if (updatedQuote != null)
            {
                _logger.Debug($"Updated: {updatedQuote}");

                var quotes = new[] { updatedQuote };
                _onNewQuotationsAction?.Invoke(quotes);
                _store?.Insert(quotes);
            }
        }

        public object GetConfiguration()
        {
            return Configuration.Default;
        }

        public void SaveConfiguration()
        {
            Configuration.Default.Save();
        }
    }
}
