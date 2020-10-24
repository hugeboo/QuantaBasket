using QuantaBasket.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuantaBasket.Core.Interfaces
{
    public interface ITradingStore
    {
        void Insert(SignalDTO signal);
        void Update(SignalDTO signal);
    }
}
