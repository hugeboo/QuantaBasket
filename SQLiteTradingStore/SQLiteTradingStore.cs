using NLog;
using QuantaBasket.Core.Contracts;
using QuantaBasket.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuantaBasket.Components.SQLiteTradingStore
{
    public sealed class SQLiteTradingStore : ITradingStore, IHaveConfiguration
    {
        private readonly string _connectionString;

        private ILogger _logger = LogManager.GetLogger("SQLiteTradingStore");

        private const string _sqlInsertSignal = "INSERT INTO Signals (Id,CreatedTime,ClassCode,SecCode,Status,Side,Qtty,Price,PriceType,ExecQtty,AvgPrice,LastUpdateTime,QuantName,MarketOrderId) " +
            "VALUES(@id,@createdTime,@classCode,@secCode,@status,@side,@qtty,@price,@priceType,@execQtty,@avgPrice,@lastUpdateTime,@quantName,@marketOrderId)";

        private const string _sqlUpdateSignal = "UPDATE Signals SET Status = @status, " +
            "ExecQtty = @execQtty, AvgPrice = @avgPrice, LastUpdateTime = @lastUpdateTime " +
            "WHERE Id = @id";

        private const string _sqlSelectSignalByIdAndDate = "SELECT Id,CreatedTime,ClassCode,SecCode,Status,Side,Qtty,Price," +
            "PriceType,ExecQtty,AvgPrice,LastUpdateTime,QuantName,MarketOrderId FROM Signals " +
            "WHERE Id = @id AND CreatedTime>=@startTime AND CreatedTime<@endTime";

        private const string _sqlInsertTrade = "INSERT INTO Trades (SignalId,MarketOrderId,MarketTradeId,MarketDateTime," +
                       "ClassCode,SecCode,Side,Qtty,Price) VALUES(" +
                       "@signalId,@marketOrderId,@marketTradeId,@marketDateTime,@classCode,@secCode,@side,@qtty,@price)";

        public SQLiteTradingStore() : this(true)
        {
        }

        public SQLiteTradingStore(bool createIfNotExist)
        {
            _connectionString = Configuration.Default.ConnectionString;

            if (createIfNotExist)
                CreateIfNotExists();
        }

        public void Insert(ISignal signal)
        {
            if (signal == null) throw new ArgumentNullException(nameof(signal));

            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();

                var parameters = MakeParameters(signal);

                var com = connection.CreateCommand();
                com.CommandText = _sqlInsertSignal;
                com.Parameters.AddRange(parameters);
                com.Prepare();

                _logger.Trace($"INSERT Signal: {signal}");

                com.ExecuteNonQuery();
            }
        }

        public void Update(ISignal signal)
        {
            if (signal == null) throw new ArgumentNullException(nameof(signal));

            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();

                var parameters = MakeParametersForUpdate(signal);

                var com = connection.CreateCommand();
                com.CommandText = _sqlUpdateSignal;
                com.Parameters.AddRange(parameters);
                com.Prepare();

                _logger.Trace($"UPDATE Signal: {signal}");

                com.ExecuteNonQuery();
            }
        }

        private SQLiteParameter[] MakeParameters(ISignal signal)
        {
            return new[] {
                        new SQLiteParameter("@id", signal.Id),
                        new SQLiteParameter("@createdTime", signal.CreatedTime),
                        new SQLiteParameter("@classCode", signal.ClassCode),
                        new SQLiteParameter("@secCode", signal.SecCode),
                        new SQLiteParameter("@status", signal.Status.ToString()),
                        new SQLiteParameter("@side", signal.Side.ToString()),
                        new SQLiteParameter("@qtty", signal.Qtty),
                        new SQLiteParameter("@price", signal.Price),
                        new SQLiteParameter("@priceType", signal.PriceType.ToString()),
                        new SQLiteParameter("@execQtty", signal.ExecQtty),
                        new SQLiteParameter("@avgPrice", signal.AvgPrice),
                        new SQLiteParameter("@lastUpdateTime", signal.LastUpdateTime),
                        new SQLiteParameter("@quantName", signal.QuantName),
                        new SQLiteParameter("@marketOrderId", signal.MarketOrderId ?? string.Empty),
                    };
        }

        private SQLiteParameter[] MakeParametersForUpdate(ISignal signal)
        {
            return new[] {
                        new SQLiteParameter("@id", signal.Id),
                        new SQLiteParameter("@status", signal.Status.ToString()),
                        new SQLiteParameter("@execQtty", signal.ExecQtty),
                        new SQLiteParameter("@avgPrice", signal.AvgPrice),
                        new SQLiteParameter("@lastUpdateTime", signal.LastUpdateTime),
                        new SQLiteParameter("@marketOrderId", signal.MarketOrderId ?? string.Empty),
                    };
        }

        public void Insert(TradeDTO trade)
        {
            if (trade == null) throw new ArgumentNullException(nameof(trade));

            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();

                var parameters = new[] {
                        new SQLiteParameter("@signalId", trade.SignalId),
                        new SQLiteParameter("@marketOrderId", trade.MarketOrderId),
                        new SQLiteParameter("@marketTradeId",trade.MarketTradeId),
                        new SQLiteParameter("@marketDateTime", trade.MarketDateTime),
                        new SQLiteParameter("@classCode", trade.ClassCode),
                        new SQLiteParameter("@secCode", trade.SecCode),
                        new SQLiteParameter("@side", trade.Side.ToString()),
                        new SQLiteParameter("@qtty", trade.Qtty),
                        new SQLiteParameter("@price", trade.Price),
                    };

                var com = connection.CreateCommand();
                com.CommandText = _sqlInsertTrade;
                com.Parameters.AddRange(parameters);
                com.Prepare();

                _logger.Trace($"INSERT Trade: {trade}");

                com.ExecuteNonQuery();
            }
        }

        public ISignal GetSignalByIdAndDate(string signalId, DateTime date)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();

                var parameters = new[]
                {
                    new SQLiteParameter("@id", signalId),
                    new SQLiteParameter("@startTime", date.Date),
                    new SQLiteParameter("@endTime", date.Date.AddDays(1)),
                };

                var com = connection.CreateCommand();
                com.CommandText = _sqlSelectSignalByIdAndDate;
                com.Parameters.AddRange(parameters);

                using (var r = com.ExecuteReader())
                {
                    if (r.Read())
                    {
                        var signal = new SignalDTO
                        {
                            Id = r.GetString(0),
                            CreatedTime = r.GetDateTime(1),
                            ClassCode = r.GetString(2),
                            SecCode = r.GetString(3),
                            Status = (SignalStatus)Enum.Parse(typeof(SignalStatus), r.GetString(4)),
                            Side = (SignalSide)Enum.Parse(typeof(SignalSide), r.GetString(5)),
                            Qtty = r.GetInt64(6),
                            Price = r.GetDecimal(7),
                            PriceType = (PriceType)Enum.Parse(typeof(PriceType), r.GetString(8)),
                            ExecQtty = r.GetInt64(9),
                            AvgPrice = r.GetDecimal(10),
                            LastUpdateTime = r.GetDateTime(11),
                            QuantName = r.GetString(12),
                            MarketOrderId = r.GetString(13)
                        };
                        return signal;
                    }
                }
                return null;
            }
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
