using QuantaBasket.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuantaBasket.Core.Extensions
{
    public static class SignalExtensions
    {
        public static INewSignal ClassCode(this INewSignal s, string classCode)
        {
            s.ClassCode = classCode;
            return s;
        }

        public static INewSignal SecCode(this INewSignal s, string secCode)
        {
            s.SecCode = secCode;
            return s;
        }

        public static INewSignal Side(this INewSignal s, SignalSide side)
        {
            s.Side = side;
            return s;
        }

        public static INewSignal Qtty(this INewSignal s, long qtty)
        {
            s.Qtty = qtty;
            return s;
        }

        public static INewSignal Price(this INewSignal s, decimal price)
        {
            s.Price = price;
            return s;
        }

        public static INewSignal PriceType(this INewSignal s, PriceType priceType)
        {
            s.PriceType = priceType;
            return s;
        }
    }
}
