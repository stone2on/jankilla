using CsvHelper.Configuration;
using Jankilla.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jankilla.Core.Converters.ClassMaps
{
    public class DeviceMap<T> : ClassMap<T> where T : Device
    {
        protected int i = 0;
        public DeviceMap()
        {
            Map(m => m.Discriminator).Index(++i);
            Map(m => m.ID).Index(++i);
            Map(m => m.Name).Index(++i);
            Map(m => m.Path).Index(++i);
            Map(m => m.Description).Index(++i);
            Map(m => m.DriverID).Index(++i);
        }
    }
}
