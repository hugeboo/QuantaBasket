﻿using NUnit.Framework;
using QuantaBasket.Basket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuantaBasketTests
{
    [TestFixture]
    class BasketTests
    {
#if DEBUG
        [Test]
        public void BrowseQuantas()
        {
            var lst = QuantBrowser.Browse();
            Assert.GreaterOrEqual(1, lst.Count());
            var t = Activator.CreateInstance(lst.First());
            Assert.NotNull(t);
        }
#endif
    }
}
