using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jankilla.Driver.MitsubishiMxComponent.Converters.Csv
{
    public class MitsubishiMxComponentDeviceMap : ClassMap<MitsubishiMxComponentDevice>
    {
        public MitsubishiMxComponentDeviceMap()
        {
            int i = -1;

            Map(m => ClassType.Name).Index(++i);

            Map(m => m.Discriminator).Index(++i);
            Map(m => m.ID).Index(++i);
            Map(m => m.Name).Index(++i);
            Map(m => m.Path).Index(++i);
            Map(m => m.Description).Index(++i);
            Map(m => m.DriverID).Index(++i);
        }
    }
}
