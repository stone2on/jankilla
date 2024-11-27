using CsvHelper.Configuration;
using Jankilla.Core.Contracts.Tags;
using Jankilla.Core.Tags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jankilla.Core.Converters.ClassMaps
{
    internal class BooleanArrayTagMap<T> : TagMap<T> where T : ArrayTag<BooleanTag>
    {
        public BooleanArrayTagMap()
        {
            Map(m => m.Tags).Index(++i); 
        }
    }

    internal class StringArrayTagMap<T> : TagMap<T> where T : ArrayTag<StringTag>
    {
        public StringArrayTagMap()
        {
            Map(m => m.Tags);
        }
    }

    internal class ShortArrayTagMap<T> : TagMap<T> where T : ArrayTag<ShortTag>
    {
        public ShortArrayTagMap()
        {
            Map(m => m.Tags);
        }
    }

    internal class IntArrayTagMap<T> : TagMap<T> where T : ArrayTag<IntTag>
    {
        public IntArrayTagMap()
        {
            Map(m => m.Tags);
        }
    }

    internal class UIntArrayTagMap<T> : TagMap<T> where T : ArrayTag<UIntTag>
    {
        public UIntArrayTagMap()
        {
            Map(m => m.Tags);
        }
    }

    internal class FloatArrayTagMap<T> : TagMap<T> where T : ArrayTag<FloatTag>
    {
        public FloatArrayTagMap()
        {
            Map(m => m.Tags);
        }
    }

    internal class DoubleArrayTagMap<T> : TagMap<T> where T : ArrayTag<DoubleTag>
    {
        public DoubleArrayTagMap()
        {
            Map(m => m.Tags);
        }
    }

    internal class UShortArrayTagMap<T> : TagMap<T> where T : ArrayTag<UShortTag>
    {
        public UShortArrayTagMap()
        {
            Map(m => m.Tags);
        }
    }

    internal class LongArrayTagMap<T> : TagMap<T> where T : ArrayTag<LongTag>
    {
        public LongArrayTagMap()
        {
            Map(m => m.Tags);
        }
    }

    internal class ULongArrayTagMap<T> : TagMap<T> where T : ArrayTag<ULongTag>
    {
        public ULongArrayTagMap()
        {
            Map(m => m.Tags);
        }
    }

    internal class ComplexArrayTagMap<T> : TagMap<T> where T : ArrayTag<ComplexTag>
    {
        public ComplexArrayTagMap()
        {
            Map(m => m.Tags);
        }
    }

}
