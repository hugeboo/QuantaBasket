using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuantaBasket.Core.Interfaces
{
    public interface ITradingEngine
    {
        IQuantSignal CreateSignal(string quantName);
        void SendSignal(IQuantSignal signal);
    }
}
