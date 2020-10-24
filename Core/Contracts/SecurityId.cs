using System;using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuantaBasket.Core.Contracts
{
    /// <summary>
    /// Идентификатор ценной бумаги
    /// </summary>
    public sealed class SecurityId : ICloneable
    {
        /// <summary>
        /// Код класса
        /// </summary>
        public string ClassCode { get; set; }

        /// <summary>
        /// Код бумаги (тикер)
        /// </summary>
        public string SecurityCode { get; set; }

        public SecurityId()
        {
        }

        public SecurityId(string classCode, string secCode)
        {
            ClassCode = classCode;
            SecurityCode = secCode;
        }

        /// <summary>
        /// Клонирование с приведением типа
        /// </summary>
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
            return $"{ClassCode} {SecurityCode}";
        }

        public override int GetHashCode()
        {
            return (ClassCode ?? "").GetHashCode() ^ (SecurityCode ?? "").GetHashCode();
        }
    }
}
