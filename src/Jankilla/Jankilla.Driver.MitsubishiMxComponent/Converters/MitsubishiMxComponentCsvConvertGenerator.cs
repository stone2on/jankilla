using CsvHelper.Configuration;
using Jankilla.Core.Converters.Base;
using Jankilla.Driver.MitsubishiMxComponent.Converters.Csv;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jankilla.Driver.MitsubishiMxComponent.Converters
{
    public class MitsubishiMxComponentCsvConvertGenerator : IRequiredCsvConvertGenerator
    {
        public IEnumerable<ClassMap> GenerateCsvConverters()
        {
            var maps = new List<ClassMap>();
            maps.Add(new MitsubishiMxComponentDriverMap());
            //var a = new MitsubishiMxComponentDriverMap();
            return maps;
        }
    }
}
