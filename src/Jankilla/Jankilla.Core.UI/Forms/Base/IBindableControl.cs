using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Jankilla.Core.UI.Forms.Base
{
    public interface IBindableControl<T>
    {
        Control Control { get; }
        void Bind (T obj);
    }
}
