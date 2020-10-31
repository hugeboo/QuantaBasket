using QuantaBasket.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuantaBasket.Basket
{
    public static class BasketFactory
    {
        public static IBasketEngine CreateBasket()
        {
            return new BasketEngine();
        }
    }
}
