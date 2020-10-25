using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuantaBasketGUI
{
    internal sealed class LogStore : ILogStore
    {
        private static LogStore defaultInstance = new LogStore();

        public event EventHandler<LogStoreEventArgs> NewMessage;

        public static LogStore Default
        {
            get
            {
                return defaultInstance;
            }
        }

        private LogStore()
        {
        }

        public void Add(string logMessage)
        {
            try
            {
                NewMessage?.Invoke(this, new LogStoreEventArgs { LogMessage = logMessage });
            }
            catch { }
        }
    }

    public interface ILogStore
    {
        event EventHandler<LogStoreEventArgs> NewMessage;
    }

    public sealed class LogStoreEventArgs : EventArgs
    {
        public string LogMessage { get; set; }
    }
}
