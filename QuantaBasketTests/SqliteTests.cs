using NUnit.Framework;
using QuantaBasket.SQLiteL1QuotationStore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuantaBasketTests
{
    [TestFixture]
    class SqliteTests
    {
        [Test]
        public void OnCreateNewDatabase_Successfully()
        {
            var filename = Path.GetTempFileName();
            var cs = $"Data Source = {filename};Version=3;";
            DbUtils.CreateDb(cs);
        }
    }
}
