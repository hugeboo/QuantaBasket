using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuantaBasket.Components.SQLiteL1QuotationStore
{
    public sealed class Configuration
    {
        private const string FileName = "SQLiteL1QuotationStore.dll.json";

        #region Singleton

        private static Configuration defaultInstance;
        private static object _syncObj = new object();

        public static Configuration Default
        {
            get
            {
                if (defaultInstance == null)
                {
                    lock (_syncObj)
                    {
                        if (defaultInstance == null)
                        {
#if DEBUG
                            var json0 = JsonConvert.SerializeObject(new Configuration(), Formatting.Indented);
                            File.WriteAllText(FileName, json0);
#endif
                            var json = File.ReadAllText(FileName);
                            defaultInstance = JsonConvert.DeserializeObject<Configuration>(json);
                        }
                    }
                }
                return defaultInstance;
            }
        }

        public void Save()
        {
            var json = JsonConvert.SerializeObject(this, Formatting.Indented);
            File.WriteAllText(FileName, json);
        }

        #endregion

        [Category("Basic")]
        [DefaultValue("Data Source=d:\\temp\\QuantaBasketL1_v2.db;Version=3;")]
        public string ConnectionString { get; set; } = "Data Source=d:\\temp\\QuantaBasketL1_v2.db;Version=3;";

        [Category("Instance")]
        [JsonIgnore]
        public string InstanceType => "SQLiteL1QuotationStore";
    }
}
