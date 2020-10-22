using QuantaBasket.Core.Interfaces;
using QuantaBasket.Core.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QantaBasket.Basket
{
    public sealed class BasketEngine : IBasketService
    {
        public void RegisterMessageProcessor(Action<AMessage> messageProcessor)
        {
            throw new NotImplementedException();
        }
    }
}
