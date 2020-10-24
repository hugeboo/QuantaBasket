using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuantaBasket.Core.Messages
{
    /// <summary>
    /// Сообщение переодически (1 раз в секунду) отсылваемое кванту
    /// Может испоьзоваться для выполнения каких-то фоновых задач
    /// Если квант задержит выполнение этого сообщения более, чем на 1 секунду, новые сообщения будут копитбся в очереди
    /// и потом отправлены скопом
    /// </summary>
    public sealed class TimerMessage : AMessage
    {
        /// <summary>
        /// Текущее системное время (не рыночное)
        /// </summary>
        public DateTime DateTime { get; set; }
    }
}
