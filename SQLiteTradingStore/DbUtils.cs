using NLog;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuantaBasket.Components.SQLiteTradingStore
{
    internal static class DbUtils
    {
        public static void CreateDb(ILogger logger, string connectionString)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                DoCommand(logger, connection, "DROP INDEX IF EXISTS Signal_DateTime_INDX");
                DoCommand(logger, connection, "DROP TABLE IF EXISTS Signals");
                DoCommand(logger, connection, "CREATE TABLE Signals (" +
                    "Id             TEXT     PRIMARY KEY NOT NULL UNIQUE," +
                    "CreatedTime    DATETIME NOT NULL," +
                    "ClassCode      TEXT     NOT NULL," +
                    "SecCode        TEXT     NOT NULL," +
                    "Status         TEXT     NOT NULL," +
                    "Side           TEXT     NOT NULL," +
                    "Qtty           NUMERIC  NOT NULL," +
                    "Price          DECIMAL  NOT NULL," +
                    "PriceType      TEXT     NOT NULL," +
                    "ExecQtty       NUMERIC  NOT NULL," +
                    "AvgPrice       DECIMAL  NOT NULL," +
                    "LastUpdateTime DATETIME NOT NULL," +
                    "QuantName      TEXT     NOT NULL," +
                    "MarketOrderId  TEXT     NOT NULL)");
                DoCommand(logger, connection, "CREATE UNIQUE INDEX Signal_DateTime_INDX ON Signals (Id, CreatedTime ASC)");

                DoCommand(logger, connection, "DROP INDEX IF EXISTS Trades_SignalId_INDX");
                DoCommand(logger, connection, "DROP INDEX IF EXISTS Trades_ClassCode_SecCode_Time_INDX");
                DoCommand(logger, connection, "DROP TABLE IF EXISTS Trades");
                DoCommand(logger, connection, "CREATE TABLE Trades (" +
                    "SignalId       TEXT     NOT NULL," +
                    "MarketOrderId  TEXT     NOT NULL," +
                    "MarketTradeId  TEXT     NOT NULL," +
                    "MarketDateTime DATETIME NOT NULL," +
                    "ClassCode      TEXT     NOT NULL," +
                    "SecCode        TEXT     NOT NULL," +
                    "Side           TEXT     NOT NULL," +
                    "Size           NUMERIC  NOT NULL," +
                    "Price          DECIMAL  NOT NULL)");
                DoCommand(logger, connection, "CREATE INDEX Trades_SignalId_INDX ON Trades (SignalId)");
                DoCommand(logger, connection, "CREATE INDEX Trades_ClassCode_SecCode_Time_INDX ON Trades (MarketDateTime,ClassCode,SecCode)");
            }
        }

        public static void DoCommand(ILogger logger, SQLiteConnection connection, string sql)
        {
            using (var c = connection.CreateCommand())
            {
                logger?.Debug($"Execute: {sql}");
                c.CommandText = sql;
                c.ExecuteNonQuery();
            }
        }
    }
}
