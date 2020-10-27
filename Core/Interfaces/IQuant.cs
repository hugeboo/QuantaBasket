using QuantaBasket.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuantaBasket.Core.Interfaces
{
    /// <summary>
    /// Интерфейс кванта
    /// Кванты идентифицируются в системе менно по этому интерфейсу
    /// </summary>
    public interface IQuant : IDisposable
    {
        /// <summary>
        /// Имя кванта
        /// В системе могут существовать два одинаковых кванта, но с разными именами
        /// Например, один одной бумагой торгует, второй другой. Имя бумаги должно входить в имя кванта.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Набор бумаг, которые интересны кванту
        /// Квант получает поток котировок в соответствии с этим списком
        /// </summary>
        HashSet<SecurityId> Securities { get; }

        /// <summary>
        /// Текущий статус кванта
        /// </summary>
        QuantStatus Status { get; }

        /// <summary>
        /// Инициализация кванта
        /// Вызывается движком после создания кванта
        /// </summary>
        void Init(IBasketService basketService);
    }

    public enum QuantStatus
    {
        Disabled,
        Idle,
        Active,
        Error
    }
}
