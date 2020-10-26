using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuantaBasket.Core.Interfaces
{
    /// <summary>
    /// Интерфейс объекта, который имеет конфигурацию и готов предоставить ее для редактирования
    /// Используется GUI, конфигурация отображается в PropertyGrid
    /// </summary>
    public interface IHaveConfiguration
    {
        /// <summary>
        /// Предоставить текущую конфигурацию для редактирования
        /// Не копию, а именно действующий объект
        /// Редактирование происходит в RealTime в PropertyGrid
        /// </summary>
        object GetConfiguration();

        /// <summary>
        /// Сохранить (в файле на диске) изменения в объекте, который был ранее передан методом GetConfiguration
        /// </summary>
        void SaveConfiguration();
    }
}
