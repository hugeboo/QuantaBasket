using NLog;
using QuantaBasket.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuantaBasket.Components.QLuaL1QuotationProvider
{
    public static class ReceiverJsonString
    {
        public static string Receive(ILogger logger, Func<byte[], int> receiveFunc)
        {
            byte[] buffer = new byte[2048];
            int i = 0;
            bool inMessage = false;

            while (true)
            {
                var b = new byte[1];

                try
                {
                    receiveFunc(b);
                }
                catch (Exception ex)
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
                    }
                    else if (inMessage && b[0] == 125) // '}'
                    {
                        buffer[i++] = b[0];
                        var m = Encoding.UTF8.GetString(buffer, 0, i);
                        logger?.Trace($"Received: {m}");
                        return m;
                    }
                    else if (inMessage)
                    {
                        buffer[i++] = b[0];
                    }
                }
                catch (Exception ex)
                {
                    throw new BadMessageException(ex.Message, ex);
                }
            }
        }
    }
}
