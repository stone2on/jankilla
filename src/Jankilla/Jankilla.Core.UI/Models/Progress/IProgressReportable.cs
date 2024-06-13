using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jankilla.Core.UI.Models
{
    public interface IProgressReportable
    {
        IProgress<ProgressReportModel> Progress { get; set; }
    }
}
