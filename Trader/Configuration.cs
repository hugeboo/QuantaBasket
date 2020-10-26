using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace QuantaBasket.Trader
{
    public sealed class Configuration
    {
        private const string FileName = "Trader.dll.json";

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
                            var dir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                            var filename = Path.Combine(dir, FileName);
#if DEBUG
                            var json0 = JsonConvert.SerializeObject(new Configuration(), Formatting.Indented);
                            File.WriteAllText(filename, json0);
#endif
                            var json = File.ReadAllText(filename);
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

        [Category("Instance")]
        [JsonIgnore]
        public string InstanceType => "TradingEngine";
    }
}
