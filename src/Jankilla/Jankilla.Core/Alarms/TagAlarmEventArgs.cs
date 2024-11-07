using Jankilla.Core.Contracts.Tags;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Jankilla.Core.Alarms
{
    public sealed class TagAlarmEventArgs : EventArgs
    {
        public DateTime Time { get; set; }
        public Tag Tag { get; set; }
        //public IReadOnlyList<TagAlarm> SubAlarms { get; set; }
        public bool IsOn { get; set; }
    }
}
