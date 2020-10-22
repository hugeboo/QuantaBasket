using QuantaBasket.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuantaBasket.Core.Interfaces
{
    public interface IQuant
    {
        string Name { get; }
        HashSet<SecurityId> Securities { get; }
        IBasketService BasketService { set; }
    }
}
