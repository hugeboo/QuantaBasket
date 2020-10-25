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

namespace QuantaBasket.Components.QLuaL1QuotationProvider
{
    public sealed class Configuration
    {
        private const string FileName = "QLuaL1QuotationProvider.json";

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
        [DefaultValue("127.0.0.1")]
        [Description("Address of QUIL Lua Server")]
        public string QLuaAddr { get; set; }

        [Category("Basic")]
        [DefaultValue(3585)]
        public int QLuaPort { get; set; }

        [Category("Basic")]
        [Editor(typeof(MultilineStringEditor), typeof(UITypeEditor))]
        public string Securities { get; set; }

        [Category("Instance")]
        [JsonIgnore]
        public string InstanceType => "QLuaL1QuotationProvider";
    }
}
