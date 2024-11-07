using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jankilla.Core.Alarms
{
    public enum ENumericAlarmCondition
    {
        Equals,
        NotEquals, 
        LessThan, 
        LessThanOrEqual,
        GreaterThan,
        GreaterThanOrEqual,
        Between
        
    }
}
