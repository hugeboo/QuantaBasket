using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace QuantaBasket.Core.Utils
{
    public class ConfigurationSingleton<T> where T : class, new()
    {
        private static T _instance;

        protected ConfigurationSingleton()
        {
        }

        private static T CreateInstance()
        {
            var dir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var filename = Path.Combine(dir, GetFileName(typeof(T)));
            if (!File.Exists(filename))
            {
                var json0 = JsonConvert.SerializeObject(new T(), Formatting.Indented);
                File.WriteAllText(filename, json0);
            }
            var json = File.ReadAllText(filename);
            return JsonConvert.DeserializeObject<T>(json);
        }

        public static T Instance
        {
            get
            {
                if (_instance == null)_instance = CreateInstance();
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
    }

    public sealed class ConfigurationAttribute : Attribute
    {
        public string FileName { get; set; }

        public ConfigurationAttribute() { }
        public ConfigurationAttribute(string fileName)
        {
            FileName = fileName;
        }
    }
}
