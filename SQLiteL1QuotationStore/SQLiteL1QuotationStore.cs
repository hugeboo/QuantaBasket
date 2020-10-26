using NLog;
using QuantaBasket.Core.Contracts;
using QuantaBasket.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuantaBasket.Components.SQLiteL1QuotationStore
{
    public sealed class SQLiteL1QuotationStore : IL1QuotationStore, IHaveConfiguration
    {
        private readonly string _connectionString;

        private ILogger _logger = LogManager.GetLogger("SQLiteL1QuotationStore");

        private const string _sqlInsert = "INSERT INTO Quatations (ClassCode,SecCode,DateTime,Bid,Ask,Last,Volume,DVolume,Changes) " +
            "VALUES(@classCode,@secCode,@dateTime,@bid,@ask,@last,@volume,@dVolume,@changes)";

        private const string _sqlSelect = "SELECT Id,ClassCode,SecCode,DateTime,Bid,Ask,Last,Volume,DVolume,Changes FROM Quatations " +
            "WHERE ClassCode=@classCode AND SecCode=@secCode AND DateTime>=@dateTimeFrom AND DateTime<=@dateTimeTo ORDER BY DateTime,Id";

        public SQLiteL1QuotationStore() : this(true)
        {
        }

        public SQLiteL1QuotationStore(bool createIfNotExist)
        {
            _connectionString = Configuration.Default.ConnectionString;

            if (createIfNotExist)
                CreateIfNotExists();
        }

        public void Insert(IEnumerable<L1Quotation> quotions)
        {
            if (quotions == null) throw new ArgumentNullException(nameof(quotions));

            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();

                foreach (var q in quotions)
                {
                    var parameters = new[] { 
                        new SQLiteParameter("@classCode", q.Security.ClassCode),
                        new SQLiteParameter("@secCode", q.Security.SecurityCode),
                        new SQLiteParameter("@dateTime", q.DateTime),
                        new SQLiteParameter("@bid", q.Bid),
                        new SQLiteParameter("@ask", q.Ask),
                        new SQLiteParameter("@last", q.Last),
                        new SQLiteParameter("@volume", q.Volume),
                        new SQLiteParameter("@dVolume", q.DVolume),
                        new SQLiteParameter("@changes", q.Changes),
                    };

                    var com = connection.CreateCommand();
                    com.CommandText = _sqlInsert;
                    com.Parameters.AddRange(parameters);
                    com.Prepare();

                    _logger.Trace($"INSERT: {q}");

                    com.ExecuteNonQuery();
                }
            }
        }

        public IEnumerable<L1Quotation> Select(L1QuotationFilter filter)
        {
            throw new NotImplementedException();
        }

        private void CreateIfNotExists()
        {
            //Data Source=d:\temp\QuantaBasketL1.db;Version=3;
            var i1 = _connectionString.IndexOf("Data Source=");
            var i2 = _connectionString.IndexOf(';', i1 + 12);
            var fileName = _connectionString.Substring(i1 + 12, i2 - i1 - 12);
            if (!File.Exists(fileName))
            {
                DbUtils.CreateDb(_logger, _connectionString);
            }
        }

        public object GetConfiguration()
        {
            return Configuration.Default;
        }

        public void SaveConfiguration()
        {
            Configuration.Default.Save();
        }
    }
}
