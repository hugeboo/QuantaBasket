using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuantaBasket.Core.Interfaces
{
    public interface IStreamDataFunction<Tin, Tout>
    {
        void Add(Tin sourceData);
        void RegisterCallback(Action<Tout> processOutputData);
        void Reset();
    }
}
