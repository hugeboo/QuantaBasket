using NUnit.Framework;
using QuantaBasket.Core.Exceptions;
using QuantaBasket.QLuaL1QuotationProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuantaBasketTests
{
    [TestFixture]
    class QLuaL1QuotationProviderTests
    {
        private string M1_true = "{\"class\":\"TQBR\",\"ask\":\"165.410000\",\"last\":\"165.410000\",\"bid\":\"165.400000\",\"time\":\"13:36:22\",\"voltoday\":\"13520850.000000\",\"sec\":\"GAZP\"}";
        private string M2_withoutBegin = "\"class\":\"TQBR\",\"ask\":\"4262.500000\",\"last\":\"4260.000000\",\"bid\":\"4260.000000\",\"time\":\"13:36:22\",\"voltoday\":\"364401.000000\",\"sec\":\"LKOH\"}";
        private string M3_withoutEnd = "{\"class\":\"TQBR\",\"ask\":\"4262.500000\",\"last\":\"4260.000000\",\"bid\":\"4260.000000\",\"time\":\"13:36:22\",\"voltoday\":\"364401.000000\",\"sec\":\"LKOH\"";

        [Test]
        public void Receive_TrueMessage_Successfully()
        {
            int i = 0;
            var s = ReceiverJsonString.Receive(null, (b) => b[0] = (byte)M1_true[i++]);
            Assert.AreEqual(M1_true, s);
        }

        [Test]
        public void Receive_MessageWithoutBegin_Exception()
        {
            int i = 0;
            Assert.Throws(typeof(TransportException), () => 
                ReceiverJsonString.Receive(null, (b) => b[0] = (byte)M2_withoutBegin[i++]));
        }

        [Test]
        public void Receive_MessageWithoutEnd_Exception()
        {
            int i = 0;
            Assert.Throws(typeof(ConnectionLostException), () =>
                ReceiverJsonString.Receive(null, (b) => b[0] = i< M3_withoutEnd.Length ? (byte)M3_withoutEnd[i++] : (byte)0));
        }
    }
}
