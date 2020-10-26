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

        private const string _sqlInsertSignal = "INSERT INTO Signals (Id,CreatedTime,ClassCode,SecCode,Status,Side,Qtty,Price,PriceType,ExecQtty,AvgPrice,LastUpdateTime,QuantName) "+                   
            "VALUES(@id,@createdTime,@classCode,@secCode,@status,@side,@qtty,@price,@priceType,@execQtty,@avgPrice,@lastUpdateTime,@quantName,@marketOrderId)";

        private const string _sqlUpdateSignal = "UPDATE Signals SET Status = @status, " +
            "ExecQtty = @execQtty, AvgPrice = @avgPrice, LastUpdateTime = @lastUpdateTime " +
            "WHERE Id = @id";

        public SQLiteTradingStore() : this(true)
        {
        }

        public SQLiteTradingStore(bool createIfNotExist)
        {
            _connectionString = Configuration.Default.ConnectionString;

            if (createIfNotExist)
                CreateIfNotExists();
        }

        public void Insert(SignalDTO signal)
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

                _logger.Trace($"INSERT: {signal}");

                com.ExecuteNonQuery();
            }
        }

        public void Update(SignalDTO signal)
        {
            if (signal == null) throw new ArgumentNullException(nameof(signal));

            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();

                var parameters = MakeParameters(signal);

                var com = connection.CreateCommand();
                com.CommandText = _sqlUpdateSignal;
                com.Parameters.AddRange(parameters);
                com.Prepare();

                _logger.Trace($"UPDATE: {signal}");

                com.ExecuteNonQuery();
            }
        }

        private SQLiteParameter[] MakeParameters(SignalDTO signal)
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
                        new SQLiteParameter("@marketOrderId", signal.MarketOrderId),
                    };
        }

        private SQLiteParameter[] MakeParametersForUpdate(SignalDTO signal)
        {
            return new[] {
                        new SQLiteParameter("@id", signal.Id),
                        new SQLiteParameter("@status", signal.Status.ToString()),
                        new SQLiteParameter("@execQtty", signal.ExecQtty),
                        new SQLiteParameter("@avgPrice", signal.AvgPrice),
                        new SQLiteParameter("@lastUpdateTime", signal.LastUpdateTime),
                        new SQLiteParameter("@marketOrderId", signal.MarketOrderId),
                    };
        }

        public void Insert(TradeDTO trade)
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
