using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuantaBasket.Core.Interfaces
{
    /// <summary>
    /// Интерфейс объекта, умеющего сообщать о возникающих ошибках
    /// </summary>
    public interface IErrorReporter
    {
        /// <summary>
        /// Зарешистрировать обработчик сообщений об ошибках
        /// </summary>
        void RegisterErrorProcessor(Action<ErrorReportCode, string> processError);
    }

    /// <summary>
    /// Коды ошибок
    /// </summary>
    public enum ErrorReportCode
    {
        /// <summary>
        /// Неизвестная ошибка
        /// </summary>
        Unknown,

        /// <summary>
        /// Потеря связи с тороговой системой
        /// </summary>
        ConnectionLost,

        /// <summary>
        /// Ошибка транспортного уровня
        /// </summary>
        TransportError
    }
}
