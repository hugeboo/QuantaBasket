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

namespace QuantaBasket.Components.QLuaL1QuotationProvider
{
    [Configuration("QLuaL1QuotationProvider.dll.json")]
    public sealed class Configuration : ConfigurationSingleton<Configuration>
    {
        [Category("Basic")]
        [DefaultValue("127.0.0.1")]
        public string QLuaAddr { get; set; } = "127.0.0.1";

        [Category("Basic")]
        [DefaultValue(3585)]
        public int QLuaPort { get; set; } = 3585;

        [Category("Basic")]
        [Editor(typeof(MultilineStringEditor), typeof(UITypeEditor))]
        public string Securities { get; set; } = "[{\"c\":\"TQBR\",\"s\":\"LKOH\"},{\"c\":\"*\",\"s\":\"RIZ0\"}]";

        [Category("Instance")]
        [JsonIgnore]
        public string InstanceType => "QLuaL1QuotationProvider";
    }
}
