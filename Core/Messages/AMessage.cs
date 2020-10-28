using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuantaBasket.Core.Messages
{
    /// <summary>
    /// Сообщение для нотификации кванта о событиях в системе (базовый класс)
    /// Раьота кванта построена по событийной модели
    /// </summary>
    public abstract class AMessage
    {
        private static JsonSerializerSettings _jsonSettngs = new JsonSerializerSettings
        {
            Converters = new[] { new StringEnumConverter() },
            Formatting = Formatting.Indented
        };

        public override string ToString()
        {
            return $"{this.GetType().Name}: {JsonConvert.SerializeObject(this, _jsonSettngs)}";
        }
    }
}
