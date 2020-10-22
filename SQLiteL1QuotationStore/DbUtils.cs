using NLog;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuantaBasket.SQLiteL1QuotationStore
{
    public static class DbUtils
    {
        public static void CreateDb(ILogger logger, string connectionString)
        {
            using (var connection = new SQLiteConnection (connectionString))
            {
                connection.Open();

                DoCommand(logger, connection, "DROP INDEX IF EXISTS Security_Date_INDX");
                DoCommand(logger, connection, "DROP TABLE IF EXISTS Quotations");
                DoCommand(logger, connection, "CREATE TABLE Quatations (" +
                    "Id        INTEGER    PRIMARY KEY AUTOINCREMENT," +
                    "ClassCode TEXT       NOT NULL," +
                    "SecCode   TEXT       NOT NULL," +
                    "DateTime  DATETIME   NOT NULL," +
                    "Bid       DECIMAL    NOT NULL," +
                    "Ask       DECIMAL    NOT NULL," +
                    "Last      DECIMAL    NOT NULL," +
                    "Volume    NUMERIC    NOT NULL," +
                    "DVolume   NUMERIC    NOT NULL," +
                    "Changes   INTEGER    NOT NULL)");
                DoCommand(logger, connection, "CREATE INDEX Security_Date_INDX ON Quatations ("+
                    "ClassCode,SecCode,DateTime ASC)");
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
