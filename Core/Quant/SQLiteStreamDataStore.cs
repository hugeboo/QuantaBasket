using QuantaBasket.Core.Interfaces;
using QuantaBasket.Core.Utils;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace QuantaBasket.Core.Quant
{
    public sealed class SQLiteStreamDataStore : IStreamDataStore
    {
        private readonly SQLiteConnection _connection;
        private readonly string _connectionString;

        private const string _sqlCreateTableIfNotExists = "CREATE TABLE IF NOT EXISTS {0} (" +
            "Id INTEGER PRIMARY KEY AUTOINCREMENT, {1})";

        public SQLiteStreamDataStore(string connectionString)
        {
            _connectionString = connectionString;
            _connection = new SQLiteConnection(connectionString);
            _connection.Open();
        }

        public void Dispose()
        {
            _connection.Dispose();
        }

        public IStreamDataChannel<T> OpenOrCreateChannel<T>(string channelName)
        {
            var i1 = _connectionString.IndexOf("Data Source=");
            var i2 = _connectionString.IndexOf(';', i1 + 12);
            var fileName = _connectionString.Substring(i1 + 12, i2 - i1 - 12);
            var dirName = Path.GetDirectoryName(fileName);
            if (!Directory.Exists(dirName))
            {
                Directory.CreateDirectory(dirName);
            }

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

            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach(var p in properties.Where(p=>p.CanRead))
            {
                var typeName = p.PropertyType.Name;
                switch (typeName)
                {
                    case "DateTime":
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
                        dict[p.Name] = "TEXT";
                        break;
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
        private readonly AsyncWorker<T> _worker;

        private static CultureInfo _culture = new CultureInfo("En-us");

        private const string _sqlInsert = "INSERT INTO {0} ({1}) VALUES({2})";

        public SQLiteStreamDataChannel(SQLiteConnection connection, string name, Dictionary<string, string> fields)
        {
            _connection = connection;
            _name = name;
            _fields = fields;
            _worker = new AsyncWorker<T>($"SQLiteStreamDataChannel:{name}", WorkerProc);
        }

        public void Dispose()
        {
            _worker.Dispose();
        }

        public void Write(T data)
        {
            _worker.AddItem(data);
        }

        private void WorkerProc(T data)
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

                    string s;
                    if (val is decimal)
                    {
                        s = ((decimal)val).ToString(_culture);
                    }
                    else if (val is DateTime)
                    {
                        var d = ((DateTime)val).ToString("yyyy-MM-dd HH:mm:ss.fff");
                        s = $"'{d}'";
                    }
                    else if (val is long)
                    {
                        s = val.ToString();
                    }
                    else
                    {
                        s = $"'{val.ToString()}'";
                    }

                    lstFieldValue.Add(s);
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
