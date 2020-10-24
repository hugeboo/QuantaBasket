using QuantaBasket.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuantaBasket.Core.Interfaces
{
    /// <summary>
    /// Хранилище торговых данных - сигналов, ордеров, трейдов (база данных)
    /// </summary>
    public interface ITradingStore
    {
        /// <summary>
        /// Сохранить сигнал
        /// </summary>
        /// <param name="signal">Торговый сигнал</param>
        void Insert(SignalDTO signal);

        /// <summary>
        /// Обновить сигнал
        /// </summary>
        /// <param name="signal">Торговый сигнал содержащий все актуальные данные</param>
        void Update(SignalDTO signal);
    }
}
