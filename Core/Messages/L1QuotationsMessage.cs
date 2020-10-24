using QuantaBasket.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuantaBasket.Core.Messages
{
    /// <summary>
    /// Поступление новых котировок
    /// </summary>
    public sealed class L1QuotationsMessage : AMessage
    {
        public IEnumerable<L1Quotation> Quotations { get; set; }
    }
}
