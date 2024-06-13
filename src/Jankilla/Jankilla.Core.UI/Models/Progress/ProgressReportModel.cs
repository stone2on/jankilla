using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jankilla.Core.UI.Models
{
    public class ProgressReportModel
    {
        public EProgressReportMode Mode { get; set; } = EProgressReportMode.Normal;

        public string Status { get; set; } = string.Empty;

        public int ProgressPercentage { get; set; } = 0;

        public string Message { get; set; }

    }
}
