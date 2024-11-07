using CsvHelper.Configuration;
using Jankilla.Core.Alarms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jankilla.Core.Converters.ClassMaps
{
    internal class ComplexAlarmMap : ClassMap<ComplexAlarm>
    {
        protected int i = 0;

        public ComplexAlarmMap()
        {
            Map(m => m.Discriminator).Index(++i);
            Map(m => m.ID).Index(++i);
            Map(m => m.Parent).Index(++i);
            Map(m => m.Code).Index(++i);
            Map(m => m.Name).Index(++i);
            Map(m => m.No).Index(++i);
            Map(m => m.AlarmLevel).Index(++i);
            Map(m => m.AlarmMessage).Index(++i);
        }
    }
}
