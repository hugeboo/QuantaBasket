using NUnit.Framework;
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
        public void OnCreateNewSQLiteL1QuotationStore_Successfully()
        {
            var filename = Path.GetTempFileName();
            var cs = $"Data Source = {filename};Version=3;";
            QuantaBasket.Components.SQLiteL1QuotationStore.DbUtils.CreateDb(null, cs);
        }

        [Test]
        public void OnCreateNewSQLiteTradingStore_Successfully()
        {
            var filename = Path.GetTempFileName();
            var cs = $"Data Source = {filename};Version=3;";
            QuantaBasket.Components.SQLiteTradingStore.DbUtils.CreateDb(null, cs);
        }
    }
}
