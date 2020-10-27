using QuantaBasket.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuantaBasket.Core.Extensions
{
    public static class SignalStatusExtensions
    {
        public static bool IsFinished(this SignalStatus status)
        {
            return status == SignalStatus.Rejected ||
                status == SignalStatus.Canceled ||
                status == SignalStatus.Completed;
        }

        public static bool IsActive(this SignalStatus status)
        {
            return status == SignalStatus.Sent ||
                status == SignalStatus.Open ||
                status == SignalStatus.Partial;
        }
    }
}
