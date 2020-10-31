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
    public sealed class SQLiteL1QuotationStore : IL1QuotationStore
    {
        private readonly string _connectionString;

        private ILogger _logger = LogManager.GetLogger("SQLiteL1QuotationStore");

        private const string _sqlInsert = "INSERT INTO Quatations (ClassCode,SecCode,DateTime,Bid,Ask,Last,LastSize,Volume,DVolume,Changes) " +
            "VALUES(@classCode,@secCode,@dateTime,@bid,@ask,@last,@lastSize,@volume,@dVolume,@changes)";

        private const string _sqlSelectCount = "SELECT COUNT(*) FROM Quatations";

        private const string _sqlSelectPage = "SELECT Id,ClassCode,SecCode,DateTime,Bid,Ask,Last,LastSize,Volume,DVolume,Changes FROM Quatations " +
            "LIMIT @limit OFFSET @offset";

        //private const string _sqlSelect = "SELECT Id,ClassCode,SecCode,DateTime,Bid,Ask,Last,LastSize,Volume,DVolume,Changes FROM Quatations " +
        //    "WHERE ClassCode=@classCode AND SecCode=@secCode AND DateTime>=@dateTimeFrom AND DateTime<=@dateTimeTo ORDER BY DateTime,Id";

        public SQLiteL1QuotationStore() : this(true)
        {
        }

        public SQLiteL1QuotationStore(bool createIfNotExist)
        {
            _connectionString = Configuration.Instance.ConnectionString;

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
                        new SQLiteParameter("@lastSize", q.LastSize),
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

        public int SelectCount()
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                var com = connection.CreateCommand();
                com.CommandText = _sqlSelectCount;
                return (int)(Int64)com.ExecuteScalar();
            }
        }

        public IEnumerable<L1Quotation> SelectPage(int limit, int offset)
        {
            var lstQuotes = new List<L1Quotation>();
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                var com = connection.CreateCommand();
                com.CommandText = _sqlSelectPage;
                com.Parameters.AddRange(new[]
                {
                    new SQLiteParameter("@limit", limit),
                    new SQLiteParameter("@offset", offset),
                });
                using (var r = com.ExecuteReader())
                {
                    while (r.Read())
                    {
                        var restaurant = new L1Quotation
                        {
                            Security = new SecurityId 
                            {
                                ClassCode = r.GetString(1),
                                SecurityCode = r.GetString(2)
                            },
                            DateTime = r.GetDateTime(3),
                            Bid = r.GetDecimal(4),
                            Ask = r.GetDecimal(5),
                            Last = r.GetDecimal(6),
                            LastSize = r.GetInt64(7),
                            Volume = r.GetInt64(8),
                            DVolume = r.GetInt64(9),
                            Changes = (L1QuotationChangedFlags)r.GetInt32(10)
                        };
                        lstQuotes.Add(restaurant);
                    }
                }
            }
            return lstQuotes;
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
            return Configuration.Instance;
        }

        public void SaveConfiguration()
        {
            Configuration.Instance.Save();
        }
    }
}
