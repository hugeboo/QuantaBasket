using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuantaBasket.Core.Contracts
{
    public sealed class L1QuotationFilter
    {
        public SecurityId Security { get; set; }
        public DateTime? DateTimeFrom { get; set; }
        public DateTime? DateTimeTo { get; set; }
        public override string ToString()
        {
            return $"{Security} {DateTimeFrom} {DateTimeTo}";
        }
    }
}
