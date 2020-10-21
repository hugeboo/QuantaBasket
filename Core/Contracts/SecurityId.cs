using System;using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuantaBasket.Core.Contracts
{
    public sealed class SecurityId : ICloneable
    {
        public string ClassCode { get; set; }
        public string SecurityCode { get; set; }

        public SecurityId Clone2()
        {
            return Clone() as SecurityId;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public override bool Equals(object obj)
        {
            var s = obj as SecurityId;
            return s?.ClassCode == ClassCode && s?.SecurityCode == SecurityCode;
        }

        public override string ToString()
        {
            return $"{ClassCode}#{SecurityCode}";
        }

        public override int GetHashCode()
        {
            return (ClassCode ?? "").GetHashCode() ^ (SecurityCode ?? "").GetHashCode();
        }
    }
}
