using CsvHelper.Configuration;
using Jankilla.Core.Converters.Csv;
using Jankilla.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jankilla.Driver.MitsubishiMxComponent.Converters.Csv
{
    public class MitsubishiMxComponentDriverMap : DriverMap<MitsubishiMxComponentDriver>
    {
        public MitsubishiMxComponentDriverMap()
        {
            Map().Index(0).ConstantFixed(ClassType.Name);
        }
    }
}
