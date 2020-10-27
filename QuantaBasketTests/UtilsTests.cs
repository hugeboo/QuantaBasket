using NUnit.Framework;
using QuantaBasket.Core.Contracts;
using QuantaBasket.Core.Mathx;
using QuantaBasket.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace QuantaBasketTests
{
    [TestFixture]
    class UtilsTests
    {
        [Test]
        public void UpdateL1_OldTime_FalseResult()
        {
            var q1 = new L1Quotation()
            {
                Security = new SecurityId { ClassCode = "EQBR", SecurityCode = "LKOH" },
                DateTime = DateTime.Now,
                Bid = 100m,
                Ask = 110m,
                Last = 105m,
                Volume = 666,
                Changes = L1QuotationChangedFlags.None
            };

            var q2 = q1.Clone2();
            q2.DateTime = q1.DateTime.AddHours(-1);

            Assert.False(L1QuotationUpdater.Update(q1, q2));
        }

        [Test]
        public void UpdateL1_AllFields_TrueResult()
        {
            var q1 = new L1Quotation()
            {
                Security = new SecurityId { ClassCode = "EQBR", SecurityCode = "LKOH" },
                DateTime = DateTime.Now,
                Bid = 100m,
                Ask = 110m,
                Last = 105m,
                Volume = 666,
                Changes = L1QuotationChangedFlags.None
            };

            var q2 = new L1Quotation()
            {
                Security = new SecurityId { ClassCode = "EQBR", SecurityCode = "LKOH" },
                DateTime = DateTime.Now.AddMinutes(1),
                Bid = 101m,
                Ask = 111m,
                Last = 106m,
                Volume = 667,
                Changes = L1QuotationChangedFlags.None
            };

            Assert.True(L1QuotationUpdater.Update(q1, q2));
            Assert.AreEqual(L1QuotationChangedFlags.All, q1.Changes);
        }

        //[Test]
        //public void RealTimeBarGenerator_EmptyBar()
        //{
        //    bool f = false;
        //    var bg = new RealTimeBarGenerator2(BarInterval.Sec5, (bar) =>
        //    {
        //        Assert.AreEqual(BarInterval.Sec5, bar.IntervalSec);
        //        Assert.IsTrue(bar.StartTime.TimeOfDay.TotalSeconds % 5 == 0);
        //        Assert.IsTrue(bar.Open == 0m && bar.High == 0m && bar.Low == 0m && bar.Close == 0m);
        //        Assert.IsTrue(bar.Volume == 0);
        //        f = true;
        //    });
        //    Thread.Sleep(9999);
        //    Assert.IsTrue(f);
        //}

        //[Test]
        //public void RealTimeBarGenerator_NotEmptyBar()
        //{
        //    var q = new L1Quotation
        //    {
        //        DateTime = DateTime.Now.AddMinutes(-1),
        //        Last = 100m,
        //        DVolume = 99
        //    };

        //    while (DateTime.Now.TimeOfDay.TotalSeconds % 5 > 0.001) { }

        //    bool f = false;
        //    var bg = new RealTimeBarGenerator(BarInterval.Sec5, (bar) =>
        //    {
        //        Assert.AreEqual(BarInterval.Sec5, bar.IntervalSec);
        //        Assert.IsTrue(bar.StartTime.TimeOfDay.TotalSeconds % 5 == 0);
        //        Assert.IsTrue(bar.Open == 1000m && bar.High == 1000m && bar.Low == 1m && bar.Close == 66m);
        //        Assert.IsTrue(bar.Volume == 396);
        //        f = true;
        //    });

        //    bg.AddQuotation(q.Clone2());

        //    Thread.Sleep(1000);

        //    q.DateTime = DateTime.Now;
        //    q.Last = 1000m;
        //    bg.AddQuotation(q.Clone2());
        //    q.DateTime = DateTime.Now;
        //    q.Last = 1m;
        //    bg.AddQuotation(q.Clone2());
        //    q.DateTime = DateTime.Now;
        //    q.Last = 100m;
        //    bg.AddQuotation(q.Clone2());
        //    q.DateTime = DateTime.Now;
        //    q.Last = 66m;
        //    bg.AddQuotation(q.Clone2());

        //    Thread.Sleep(8000);
        //    Assert.IsTrue(f);
        //}

        [Test]
        public void CalculateSimpleMovingAverage()
        {
            var src = new decimal[] { 25m, 8m, 65m, 4m, 95m, 75m, 15m, 35m, 2m, 8m, 65m };
            var lstDst = new List<decimal>();
            var sma = new SMA(4, (d) => lstDst.Add(d));
            foreach (var d in src) sma.Add(d);
            var dst = lstDst.ToArray();
            CollectionAssert.AreEqual(new decimal[] { 25.5m, 43m, 59.75m, 47.25m, 55m, 31.75m, 15m, 27.5m }, dst);
            Assert.AreEqual(11, sma.TotalSourcePointCount);
        }
    }
}
