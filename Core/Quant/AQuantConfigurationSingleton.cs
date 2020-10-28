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

namespace QuantaBasket.Core.Quant
{
    public abstract class AQuantConfigurationSingleton<T> where T : class, new()
    {
        private static T _instance;

        protected AQuantConfigurationSingleton()
        {
        }

        private static T CreateInstance()
        {
            var dir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var filename = Path.Combine(dir, GetFileName(typeof(T)));
#if DEBUG
            var json0 = JsonConvert.SerializeObject(new T(), Formatting.Indented);
            File.WriteAllText(filename, json0);
#endif
            var json = File.ReadAllText(filename);
            return JsonConvert.DeserializeObject<T>(json);
        }

        public static T Instance
        {
            get
            {
                if (_instance == null) _instance = CreateInstance();
                return _instance;
            }
        }

        public void Save()
        {
            var json = JsonConvert.SerializeObject(this, Formatting.Indented);
            File.WriteAllText(GetFileName(this.GetType()), json);
        }

        private static string GetFileName(Type t)
        {
            object[] attrs = t.GetCustomAttributes(false);
            foreach (ConfigurationAttribute attr in attrs)
            {
                return attr.FileName;
            }
            throw new Exception($"Attribute 'Configuration' not found. Type: {t.FullName}");
        }

        [Category("Basic")]
        [DefaultValue(true)]
        public bool Enabled { get; set; } = true;

        [Category("Basic")]
        [Editor(typeof(MultilineStringEditor), typeof(UITypeEditor))]
        public string Securities { get; set; } = "[{\"c\":\"TQBR\",\"s\":\"LKOH\"},{\"c\":\"SPBFUT\",\"s\":\"RIZ0\"}]";

        [Category("Instance")]
        [JsonIgnore]
        public abstract string InstanceType { get; }
    }
}
