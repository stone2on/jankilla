using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Jankilla.Core.UI.WinForms.Forms.Base
{
    public interface IDataAccessControl
    {
        EControlCommand Command { get; set; }
        Control Control { get; }
        object Do(ref string errorMessage);
    }
}
