using DevExpress.XtraCharts;
using DevExpress.XtraEditors;
using Jankilla.Core.Contracts.Tags;
using Jankilla.Core.UI.WinForms.Utils;
using Jankilla.Sample.WinForms.Controls.Controllers;
using Jankilla.Sample.WinForms.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Jankilla.Sample.WinForms.Controls.Pages
{
    public partial class HistorySearchPageControl : DevExpress.XtraEditors.XtraUserControl
    {
        #region Constructor

        public HistorySearchPageControl()
        {
            InitializeComponent();
        }

        #endregion

        #region Private Helpers

        private void reloadCategories()
        {
            if (listBoxControlCategories.InvokeRequired)
            {
                listBoxControlCategories.Invoke(new MethodInvoker(delegate { listBoxControlCategories.Items.Clear(); }));
            }
            else
            {
                listBoxControlCategories.Items.Clear();
            }

            var tags = AccessManager.Instance.GetTags();
            if (tags == null)
            {
                return;
            }

            var categories = tags
                .OrderBy(t => t.No)
                .GroupBy(t => t.Category);

            foreach (var c in categories)
            {
                if (listBoxControlCategories.InvokeRequired)
                {
                    listBoxControlCategories.Invoke(new MethodInvoker(delegate { listBoxControlCategories.Items.Add(c.Key); }));
                }
                else
                {
                    listBoxControlCategories.Items.Add(c.Key);
                }
            }

            Trace.WriteLine($"categories are reloaded.");
        }

        private async Task loadTagValuesAsync(string category)
        {
            gridControlTagValue.DataSource = null;
            gridViewTagValue.Columns.Clear();

            chartControlTagValue.Series.Clear();

            EDateSearchOption option = (EDateSearchOption)comboBoxEditDateSearchOption.SelectedItem;
            var dt = await AccessManager.Instance.GetTagValuesAsync(category, option);

            if (dt == null || dt.Columns.Count == 0)
            {
                return;
            }

            foreach (DataColumn col in dt.Columns)
            {
                if (col.ColumnName == "Timestamp")
                {
                    continue;
                }

                if (col.DataType == typeof(string))
                {
                    continue;
                }

                var series = new Series(col.ColumnName, ViewType.Line);

                foreach (DataRow row in dt.Rows)
                {
                    if (row[col.ColumnName] == DBNull.Value)
                    {
                        continue;
                    }

                    series.Points.Add(new SeriesPoint(row["Timestamp"], row[col.ColumnName]));

                }

                chartControlTagValue.Series.Add(series);
            }


            if (chartControlTagValue.Series.Count > 0)
            {
                ((XYDiagram)chartControlTagValue.Diagram).AxisX.DateTimeScaleOptions.AggregateFunction = AggregateFunction.Average;

                switch (option)
                {
                    case EDateSearchOption.HOUR1:
                        ((XYDiagram)chartControlTagValue.Diagram).AxisX.DateTimeScaleOptions.MeasureUnit = DateTimeMeasureUnit.Second;
                        ((XYDiagram)chartControlTagValue.Diagram).AxisX.DateTimeScaleOptions.GridAlignment = DateTimeGridAlignment.Second;
                        break;
                    case EDateSearchOption.HOUR6:
                    case EDateSearchOption.HOUR12:
                        ((XYDiagram)chartControlTagValue.Diagram).AxisX.DateTimeScaleOptions.MeasureUnit = DateTimeMeasureUnit.Minute;
                        ((XYDiagram)chartControlTagValue.Diagram).AxisX.DateTimeScaleOptions.GridAlignment = DateTimeGridAlignment.Minute;
                        break;
                    case EDateSearchOption.DAY1:
                    case EDateSearchOption.DAY7:
                        ((XYDiagram)chartControlTagValue.Diagram).AxisX.DateTimeScaleOptions.MeasureUnit = DateTimeMeasureUnit.Hour;
                        ((XYDiagram)chartControlTagValue.Diagram).AxisX.DateTimeScaleOptions.GridAlignment = DateTimeGridAlignment.Hour;
                        break;
                    case EDateSearchOption.DAY30:
                        ((XYDiagram)chartControlTagValue.Diagram).AxisX.DateTimeScaleOptions.MeasureUnit = DateTimeMeasureUnit.Day;
                        ((XYDiagram)chartControlTagValue.Diagram).AxisX.DateTimeScaleOptions.GridAlignment = DateTimeGridAlignment.Day;
                        break;
                    case EDateSearchOption.DAY90:
                        ((XYDiagram)chartControlTagValue.Diagram).AxisX.DateTimeScaleOptions.MeasureUnit = DateTimeMeasureUnit.Week;
                        ((XYDiagram)chartControlTagValue.Diagram).AxisX.DateTimeScaleOptions.GridAlignment = DateTimeGridAlignment.Week;
                        break;
                    default:
                        break;
                }

                chartControlTagValue.Legend.MarkerMode = LegendMarkerMode.CheckBox;
            }

            simpleLabelItemUpdatedTime.Text = $"* {DateTime.Now}";

            chartControlTagValue.Refresh();

            gridControlTagValue.DataSource = dt;
            gridControlTagValue.RefreshDataSource();

            gridViewTagValue.Columns["Timestamp"].ColumnEdit = repositoryItemDateEdit1;
            gridViewTagValue.BestFitColumns();
            gridViewTagValue.MoveLast();

            Trace.WriteLine($"[{category}] Updated.");

        }

        #endregion

        #region Events

        private void onLoad(object sender, EventArgs e)
        {
            foreach (var option in Enum.GetValues(typeof(EDateSearchOption)))
            {
                comboBoxEditDateSearchOption.Properties.Items.Add(option);
            }
            comboBoxEditDateSearchOption.SelectedIndex = 0;
            chartControlTagValue.SeriesSorting = SortingMode.Ascending;
            AccessManager.Instance.ProjectReloaded += accessManager_ProjectReloaded;

            reloadCategories();
        }

        private void accessManager_ProjectReloaded(object sender, EventArgs e)
        {
            reloadCategories();
        }

        private async void treeListTags_FocusedNodeChanged(object sender, DevExpress.XtraTreeList.FocusedNodeChangedEventArgs e)
        {
            var tag = e.Node.Tag as Tag;
            if (tag == null)
            {
                return;
            }

            if (e.Node.Level == 1)
            {
                chartControlTagValue.Series.Clear();


                EDateSearchOption option = (EDateSearchOption)comboBoxEditDateSearchOption.SelectedItem;
                var values = await AccessManager.Instance.GetTagValuesAsync("", option);

                if (tag.Discriminator == Jankilla.Core.Tags.Base.ETagDiscriminator.String)
                {
                    //foreach (var val in values)
                    //{
                    //    tagValueBindingSource.Add(val);
                    //}
                    //Series strSeries = new Series(tag.Name, ViewType.Pie);

                    //strSeries.Label.TextPattern = "{VP:p0} ({V:}ea)";
                    //strSeries.LegendTextPattern = "{A} : {VP:p0}";
                    //strSeries.SeriesPointsSortingKey = SeriesPointKey.Argument;
                    //strSeries.SeriesPointsSorting = SortingMode.Ascending;

                    //var valGroups = values.GroupBy(v => v.Value);
                    //foreach (var group in valGroups)
                    //{
                    //    if (string.IsNullOrEmpty(group.Key?.ToString()))
                    //    {
                    //        strSeries.Points.Add(new SeriesPoint("[NO DATA]", new object[] { group.Count() }));
                    //    }
                    //    else
                    //    {
                    //        strSeries.Points.Add(new SeriesPoint(group.Key, new object[] { group.Count() }));
                    //    }
                    //}

                    //chartControlTagValue.Series.Add(strSeries);
                    return;
                }

                Series numSeries = new Series(tag.Name, ViewType.Line);

                //foreach (var val in values)
                //{
                //    tagValueBindingSource.Add(val);
                //    numSeries.Points.Add(new SeriesPoint(val.Timestamp, val.Value));
                //}

                chartControlTagValue.Series.Add(numSeries);

                numSeries.ArgumentScaleType = ScaleType.DateTime;

                ((LineSeriesView)numSeries.View).MarkerVisibility = DevExpress.Utils.DefaultBoolean.True;
                ((LineSeriesView)numSeries.View).LineMarkerOptions.Kind = MarkerKind.Triangle;
                ((LineSeriesView)numSeries.View).LineStyle.DashStyle = DashStyle.Dash;

                ((XYDiagram)chartControlTagValue.Diagram).EnableAxisXZooming = true;

                ((XYDiagram)chartControlTagValue.Diagram).AxisX.DateTimeScaleOptions.MeasureUnit = DateTimeMeasureUnit.Millisecond;
                ((XYDiagram)chartControlTagValue.Diagram).AxisX.DateTimeScaleOptions.GridAlignment = DateTimeGridAlignment.Millisecond;

            }
        }

        private void comboBoxEditDateSearchOption_Properties_CustomItemDisplayText(object sender, DevExpress.XtraEditors.CustomItemDisplayTextEventArgs e)
        {
            switch (e.Index)
            {
                case 0:
                    e.DisplayText = "1시간 전";
                    break;
                case 1:
                    e.DisplayText = "6시간 전";
                    break;
                case 2:
                    e.DisplayText = "12시간 전";
                    break;
                case 3:
                    e.DisplayText = "하루 전";
                    break;
                case 4:
                    e.DisplayText = "7일 전";
                    break;
                case 5:
                    e.DisplayText = "30일 전";
                    break;
                case 6:
                    e.DisplayText = "90일 전";
                    break;
                default:
                    break;
            }
        }

        private void comboBoxEditDateSearchOption_CustomDisplayText(object sender, DevExpress.XtraEditors.Controls.CustomDisplayTextEventArgs e)
        {
            if (e.Value == null)
            {
                return;
            }

            Enum.TryParse(e.Value.ToString(), out EDateSearchOption code);
            switch (code)
            {
                case EDateSearchOption.HOUR1:
                    e.DisplayText = "1시간 전";
                    break;
                case EDateSearchOption.HOUR6:
                    e.DisplayText = "6시간 전";
                    break;
                case EDateSearchOption.HOUR12:
                    e.DisplayText = "12시간 전";
                    break;
                case EDateSearchOption.DAY1:
                    e.DisplayText = "하루 전";
                    break;
                case EDateSearchOption.DAY7:
                    e.DisplayText = "7일 전";
                    break;
                case EDateSearchOption.DAY30:
                    e.DisplayText = "30일 전";
                    break;
                case EDateSearchOption.DAY90:
                    e.DisplayText = "90일 전";
                    break;
            }
        }

        #endregion

        private async void listBoxControlCategories_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxControlCategories.SelectedIndex < 0)
            {
                return;
            }

            var item = listBoxControlCategories.Items[listBoxControlCategories.SelectedIndex];
            await loadTagValuesAsync(item.ToString());
        }

        private async void listBoxControlCategories_MouseClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (listBoxControlCategories.SelectedIndex < 0)
            {
                return;
            }

            var item = listBoxControlCategories.Items[listBoxControlCategories.SelectedIndex];
            await loadTagValuesAsync(item.ToString());
        }

        private void simpleLabelItemDataTable_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right)
            {
                return;
            }

            popupMenuExport.ShowPopup(PointToScreen(e.Location));
        }

        private void barButtonItemExportToCsv_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            var path = DialogHelper.ShowSaveFileDialog(DialogHelper.FILTER_STR_CSV);
            if (path == null)
            {
                return;
            }

            gridViewTagValue.ExportToCsv(path);
        }

        private void barButtonItemExportToXlsx_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            var path = DialogHelper.ShowSaveFileDialog(DialogHelper.FILTER_STR_XLSX);
            if (path == null)
            {
                return;
            }

            gridViewTagValue.ExportToXlsx(path);
        }

        private void barButtonItemExportToPdf_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            var path = DialogHelper.ShowSaveFileDialog(DialogHelper.FILTER_STR_PDF);
            if (path == null)
            {
                return;
            }

            gridViewTagValue.ExportToPdf(path);
        }
    }

    public class TagValue
    {
        public string Category { get; set; }
        public string TagName { get; set; }
        public DateTime Timestamp { get; set; }
        public object Value { get; set; }
    }
}
