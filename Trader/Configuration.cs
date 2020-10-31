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

namespace QuantaBasket.Trader
{
    [Configuration("Config\\Trader.dll.json")]
    public sealed class Configuration : ConfigurationSingleton<Configuration>
    {
        [Category("Instance")]
        [JsonIgnore]
        public string InstanceType => "TradingEngine";
    }
}
