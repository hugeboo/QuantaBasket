using NUnit.Framework;
using QuantaBasket.Core.Contracts;
using QuantaBasket.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    }
}
