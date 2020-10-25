using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using NLog.Config;
using NLog.Targets;

namespace QuantaBasketGUI
{
    [Target("QuantaBasketGUI")]
    public sealed class NLogCustomTarget : TargetWithLayout
    {
        public NLogCustomTarget()
        {
        }

        //[RequiredParameter]
        //public string Host { get; set; }

        protected override void Write(LogEventInfo logEvent)
        {
            string logMessage = this.Layout.Render(logEvent);
            LogStore.Default.Add(logMessage);
        }
    }
}
