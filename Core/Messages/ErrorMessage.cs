using QuantaBasket.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuantaBasket.Core.Messages
{
    /// <summary>
    /// Сообщение об ошибке
    /// </summary>
    public sealed class ErrorMessage : AMessage
    {
        public ErrorReportCode ErrorCode { get; set; }
        public string Message { get; set; }
    }
}
