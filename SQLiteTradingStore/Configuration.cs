using Newtonsoft.Json;
using QuantaBasket.Core.Utils;
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

namespace QuantaBasket.Components.SQLiteTradingStore
{
    [Configuration("SQLiteTradingStore.dll.json")]
    public sealed class Configuration : ConfigurationSingleton<Configuration>
    {
        [Category("Basic")]
        [DefaultValue("Data Source=d:\\temp\\QuantaBasketTrading.db;Version=3;")]
        public string ConnectionString { get; set; } = "Data Source=d:\\temp\\QuantaBasketTrading.db;Version=3;";

        [Category("Instance")]
        [JsonIgnore]
        public string InstanceType => "SQLiteTradingStore";
    }
}
