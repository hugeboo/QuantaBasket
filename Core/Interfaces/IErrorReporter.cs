using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuantaBasket.Core.Interfaces
{
    public interface IErrorReporter
    {
        void OnError(Action<ErrorReportCode, string> processError);
    }

    public enum ErrorReportCode
    {
        ConnectionLost
    }
}
