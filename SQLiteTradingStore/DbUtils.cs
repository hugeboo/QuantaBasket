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
                    "QuantName      TEXT     NOT NULL)");
                DoCommand(logger, connection, "CREATE UNIQUE INDEX Signal_DateTime_INDX ON Signals (Id, CreatedTime ASC)");
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
