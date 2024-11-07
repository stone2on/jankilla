using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jankilla.Core.Alarms
{
    public sealed class ComplexAlarmEventArgs : EventArgs
    {
        public DateTime Time { get; set; }
        public IReadOnlyList<BaseAlarm> SubAlarms { get; set; }
        public bool IsOn { get; set; }
    }
}
