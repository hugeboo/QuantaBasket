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

namespace QuantaBasket.Basket
{
    [Configuration("Config\\Basket.dll.json")]
    public sealed class Configuration : ConfigurationSingleton<Configuration>
    {
        [Category("Basic")]
        [DefaultValue("d:\\CSharp\\Quantas")]
        public string DebugQuantasPath { get; set; } = "d:\\CSharp\\Quantas";

        [Category("Basic")]
        [DefaultValue("d:\\CSharp\\QuantaRealese")]
        public string ReleaseQuantasPath { get; set; } = "d:\\CSharp\\QuantaRealese";

        [Category("Basic")]
        [DefaultValue(BasketQuotationMode.Simulator)]
        [TypeConverter(typeof(EnumConverter))]
        public BasketQuotationMode QuotationMode { get; set; } = BasketQuotationMode.Simulator;

        [Category("Instance")]
        [JsonIgnore]
        public string InstanceType => "BasketEngine";
    }

    public enum BasketQuotationMode
    {
        Production,
        Simulator
    }
}
