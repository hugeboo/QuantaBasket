using QuantaBasket.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace QuantaBasket.Core.Quant
{
    public sealed class SQLiteStreamDataStore : IStreamDataStore
    {
        private readonly SQLiteConnection _connection;

        private const string _sqlCreateTableIfNotExists = "CREATE TABLE IF NOT EXISTS {0} (" +
            "Id INTEGER PRIMARY KEY AUTOINCREMENT, {1})";

        public SQLiteStreamDataStore(string connectionString)
        {
            _connection = new SQLiteConnection(connectionString);
            _connection.Open();
        }

        public void Dispose()
        {
            _connection.Dispose();
        }

        public IStreamDataChannel<T> OpenOrCreateChannel<T>(string channelName)
        {
            var fields = GetTableFields<T>();

            using (var com = _connection.CreateCommand())
            {
                com.CommandText = string.Format(_sqlCreateTableIfNotExists,
                    channelName,
                    string.Join(",", fields.Select(f => $"{f.Key} {f.Value} NOT NULL")));
                com.ExecuteNonQuery();
            }

            return new SQLiteStreamDataChannel<T>(_connection, channelName, fields);
        }

        private Dictionary<string, string> GetTableFields<T>()
        {
            var dict = new Dictionary<string, string>();

            var properties = typeof(T).GetProperties(BindingFlags.Public);
            foreach(var p in properties.Where(p=>p.CanRead))
            {
                var typeName = p.GetType().Name;
                switch (typeName)
                {
                    case "DateType":
                        dict[p.Name] = "DATETIME";
                        break;
                    case "Int64":
                        dict[p.Name] = "NUMERIC";
                        break;
                    case "String":
                        dict[p.Name] = "TEXT";
                        break;
                    case "Decimal":
                        dict[p.Name] = "DECIMAL";
                        break;
                    default:
                        throw new InvalidOperationException($"Invalid property type: '{typeName}'");
                }
            }

            return dict;
        }
    }

    internal sealed class SQLiteStreamDataChannel<T> : IStreamDataChannel<T>
    {
        private readonly SQLiteConnection _connection;
        private readonly string _name;
        private readonly Dictionary<string, string> _fields;
        private static CultureInfo _culture = new CultureInfo("En-us");

        private const string _sqlInsert = "INSERT INTO {0} ({1}) VALUES({2})";

        public SQLiteStreamDataChannel(SQLiteConnection connection, string name, Dictionary<string, string> fields)
        {
            _connection = connection;
            _name = name;
            _fields = fields;
        }

        public void Dispose()
        {
        }

        public void Write(T data)
        {
            var properties = typeof(T).GetProperties();
            var lstFieldName = new List<string>();
            var lstFieldValue = new List<string>();

            foreach (var p in properties)
            {
                if (_fields.TryGetValue(p.Name, out string sqlType))
                {
                    lstFieldName.Add(p.Name);
                    var val = p.GetValue(data);
                    var s = val is decimal ? ((decimal)val).ToString(_culture) : val.ToString();
                    lstFieldValue.Add($"'s'");
                }
            }

            using (var com = _connection.CreateCommand())
            {
                com.CommandText = string.Format(_sqlInsert,
                    _name,
                    string.Join(",", lstFieldName),
                    string.Join(",", lstFieldValue));
                com.ExecuteNonQuery();
            }
        }
    }
}
