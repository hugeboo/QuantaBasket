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
        public static void CreateDb(string connectionString)
        {
            using (var connection = new SQLiteConnection (connectionString))
            {
                connection.Open();

                DoCommand(connection, "DROP INDEX IF EXISTS Security_Date_INDX");
                DoCommand(connection, "DROP TABLE IF EXISTS Quotations");
                DoCommand(connection, "CREATE TABLE Quatations (" +
                    "Id        INTEGER    PRIMARY KEY AUTOINCREMENT," +
                    "ClassCode TEXT       NOT NULL," +
                    "SecCode   TEXT       NOT NULL," +
                    "DateTime  DATETIME   NOT NULL," +
                    "Bid       DECIMAL    NOT NULL," +
                    "Ask       DECIMAL    NOT NULL," +
                    "Last      DECIMAL    NOT NULL," +
                    "Volume    NUMERIC    NOT NULL," +
                    "Changes   INTEGER    NOT NULL)");
                DoCommand(connection, "CREATE INDEX Security_Date_INDX ON Quatations ("+
                    "ClassCode,SecCode,DateTime ASC)");
            }
        }

        public static void DoCommand(SQLiteConnection connection, string sql)
        {
            using (var c = connection.CreateCommand())
            {
                c.CommandText = sql;
                c.ExecuteNonQuery();
            }
        }
    }
}
