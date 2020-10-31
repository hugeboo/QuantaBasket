using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuantaBasket.Core.Messages
{
    /// <summary>
    /// Сообщение переодически отсылваемое кванту, если нет другой активности
    /// Может испоьзоваться для выполнения каких-то фоновых задач
    /// </summary>
    public sealed class TimerMessage : AMessage
    {
        /// <summary>
        /// Текущее BasketTime
        /// </summary>
        public DateTime? DateTime { get; set; }
    }
}
