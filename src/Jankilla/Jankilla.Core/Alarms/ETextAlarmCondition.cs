using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jankilla.Core.Alarms
{
    public enum ETextAlarmCondition
    {
        Equals,
        NotEquals,
        Contains,
        NotContains,
        IsBlank,
        IsNotBlank,
        BeginsWith,
        EndsWith
        
    }
}
