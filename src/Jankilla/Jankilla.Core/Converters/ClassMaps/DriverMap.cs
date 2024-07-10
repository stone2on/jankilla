using CsvHelper.Configuration;
using Jankilla.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jankilla.Core.Converters.ClassMaps
{
    public class DriverMap<T> : ClassMap<T> where T : Driver
    {
        protected int i = 0;
        public DriverMap()
        {
            Map(m => m.Discriminator).Index(++i);
            Map(m => m.ID).Index(++i);
            Map(m => m.Name).Index(++i);
            Map(m => m.Path).Index(++i);
            Map(m => m.Description).Index(++i);
        }
    }
}
