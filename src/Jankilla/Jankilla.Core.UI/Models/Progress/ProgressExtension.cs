using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jankilla.Core.UI.Models
{
    public static class ProgressExtension
    {
        public static void Report(this IProgress<ProgressReportModel> progress, int percentage, string status)
        {
            Debug.Assert(progress != null);
            var report = new ProgressReportModel
            {
                ProgressPercentage = percentage,
                Status = status
            };

            progress.Report(report);
        }

        public static void ReportLog(this IProgress<ProgressReportModel> progress, string message)
        {
            var report = new ProgressReportModel
            {
                Mode = EProgressReportMode.LoggingReport,
                Message = message
            };

            progress.Report(report);
        }
    }
}
