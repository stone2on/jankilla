using CsvHelper.Configuration;
using Jankilla.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jankilla.Core.Converters.ClassMaps
{
    public class BlockMap<T> : ClassMap<T> where T : Block
    {
        protected int i = 0;
        public BlockMap()
        {
            Map(m => m.Discriminator).Index(++i);
            Map(m => m.ID).Index(++i);
            Map(m => m.Name).Index(++i);
            Map(m => m.Path).Index(++i);
            Map(m => m.Description).Index(++i);
            Map(m => m.DeviceID).Index(++i);
            Map(m => m.StartAddress).Index(++i);
            Map(m => m.BufferSize).Index(++i);
        }
    }
}
