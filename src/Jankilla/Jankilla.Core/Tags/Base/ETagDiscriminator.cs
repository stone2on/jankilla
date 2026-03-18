using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jankilla.Core.Tags.Base
{
    public enum ETagDiscriminator
    {
        Boolean = 0,
        Int = 1,
        Short = 2,
        String = 3,
        Float = 4,
        UShort = 5,
        UInt = 6,
        Long = 7,
        ULong = 8,
        Double = 9,
        Complex = 10,

        BooleanArray = 100,
        IntArray = 101,
        ShortArray = 102,
        StringArray = 103,
        FloatArray = 104,
        UShortArray = 105,
        UIntArray = 106,
        LongArray = 107,
        ULongArray = 108,
        DoubleArray = 109,
        ComplexArray = 110
    }
}
