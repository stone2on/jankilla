using CsvHelper.Configuration;
using Jankilla.Core.Contracts.Tags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jankilla.Core.Converters.ClassMaps
{
    internal class TagMap<T> : ClassMap<T> where T : Tag
    {
        protected int i = 0;
        public TagMap()
        {
            Map(m => m.Discriminator).Index(++i);
            Map(m => m.ID).Index(++i);
            Map(m => m.Name).Index(++i);
            Map(m => m.Path).Index(++i);
            Map(m => m.No).Index(++i);
            Map(m => m.Direction).Index(++i);
            Map(m => m.ByteSize).Index(++i);
            Map(m => m.ReadOnly).Index(++i);
            Map(m => m.Address).Index(++i);
            Map(m => m.Category).Index(++i);
            Map(m => m.Description).Index(++i);
            Map(m => m.BlockID).Index(++i);
            Map(m => m.Unit).Index(++i);
            Map(m => m.UseFactor).Index(++i);
            Map(m => m.Factor).Index(++i);
            Map(m => m.UseOffset).Index(++i);
            Map(m => m.Offset).Index(++i);
        }
    }
}
