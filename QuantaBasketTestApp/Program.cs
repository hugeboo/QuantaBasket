using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using QuantaBasket.QLuaL1QuotationProvider;
using QuantaBasket.SQLiteL1QuotationStore;

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
