using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using QuantaBasket.Components.QLuaL1QuotationProvider;
using QuantaBasket.Components.SQLiteL1QuotationStore;

namespace QuantaBasketTestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var store = new SQLiteL1QuotationStore();

            using (var qprovider = new QLuaL1QuotationProvider(store))
            {
                qprovider.Connect();

                while (true) { Thread.Sleep(1000); }
            }
        }
    }
}
