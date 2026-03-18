using DevExpress.Data;
using DevExpress.Utils.Menu;
using DevExpress.Utils;
using DevExpress.XtraCharts;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using Jankilla.Core.UI.WinForms.Utils;
using Jankilla.Sample.WinForms.Controls.Controllers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Jankilla.Core.Contracts.Tags;

namespace Jankilla.Sample.WinForms.Controls.Pages
{
    public partial class MainHomePageUserControl : DevExpress.XtraEditors.XtraUserControl
    {
        #region Fields

        private DXMenuItem _writeTagMenuItem;
        private DXMenuItem _tagToRealtimeMenuItem;

        private ObservableCollection<DataPoint> _dataPoints = new ObservableCollection<DataPoint>();

        private System.Timers.Timer _collectTimer = new System.Timers.Timer();

        private Tag _realtimeSelectedTag;

        private BindingSource _tagBindingSource;
        private RealTimeSource _realitmeTagSource;

        #endregion

        #region Constructor

        public MainHomePageUserControl()
        {
            InitializeComponent();
        }

        #endregion

        #region Private Helpers

        private void reloadTags()
        {
            _tagBindingSource.Clear();

            var tags = AccessManager.Instance.GetTags();
            if (tags == null)
            {
                return;
            }

            foreach (var tag in tags)
            {
                _tagBindingSource.Add(tag);
            }
        }

        #endregion

        #region Events

        private void onLoad(object sender, System.EventArgs e)
        {
            _tagBindingSource = new BindingSource(this.components);
            _tagBindingSource.DataSource = typeof(Tag);

            _realitmeTagSource = new RealTimeSource();
            _realitmeTagSource.DataSource = _tagBindingSource;

            gridControlTags.DataSource = _realitmeTagSource;

            //var imgRealtime = svgImageCollection1[5];
            //var imgEdit = svgImageCollection1[2];

            _writeTagMenuItem = new DXMenuItem("Write", tagWriteMenuItem_Click);
            _tagToRealtimeMenuItem = new DXMenuItem("To Realtime-Chart", realtimeChartSetMenuItem_Click);

            Series series = new Series();
            series.ChangeView(ViewType.Line);
            series.DataSource = _dataPoints;
            series.DataSourceSorted = true;
            series.ArgumentDataMember = "Argument";
            series.ValueDataMembers.AddRange("Value");
            chartControlRealtime.Series.Add(series);

            LineSeriesView seriesView = (LineSeriesView)series.View;
            seriesView.LastPoint.LabelDisplayMode = SidePointDisplayMode.DiagramEdge;
            seriesView.LastPoint.Label.TextPattern = "{V:f2}";

            XYDiagram diagram = (XYDiagram)chartControlRealtime.Diagram;
            diagram.AxisX.DateTimeScaleOptions.ScaleMode = ScaleMode.Continuous;
            diagram.AxisX.Label.ResolveOverlappingOptions.AllowRotate = false;
            diagram.AxisX.Label.ResolveOverlappingOptions.AllowStagger = false;
            diagram.AxisX.WholeRange.SideMarginsValue = 0;
            diagram.DependentAxesYRange = DefaultBoolean.True;
            diagram.AxisY.WholeRange.AlwaysShowZeroLevel = false;


            _collectTimer.Elapsed += _collectTimer_Elapsed;
            _collectTimer.Interval = AccessManager.Instance.Intervals;
            _collectTimer.Start();

            AccessManager.Instance.ProjectReloaded += accessManager_ProjectReloaded;
            reloadTags();

        }

        private void _collectTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (_realtimeSelectedTag == null)
            {
                return;
            }

            if (AccessManager.Instance.IsStarted == false)
            {
                return;
            }

            double val = 0;
            switch (_realtimeSelectedTag.Discriminator)
            {
                case Jankilla.Core.Tags.Base.ETagDiscriminator.Boolean:
                    val = ((bool)_realtimeSelectedTag.Value) == true ? 1 : 0;
                    break;
                case Jankilla.Core.Tags.Base.ETagDiscriminator.Int:
                    val = (double)_realtimeSelectedTag.CalibratedValue;
                    break;
                case Jankilla.Core.Tags.Base.ETagDiscriminator.Short:
                    val = (double)_realtimeSelectedTag.CalibratedValue;
                    break;
                case Jankilla.Core.Tags.Base.ETagDiscriminator.Float:
                    val = (double)_realtimeSelectedTag.CalibratedValue;
                    break;
                default:
                    break;
            }

            _dataPoints.Add(new DataPoint(DateTime.Now, val));

            if (_dataPoints.Count > 200)
            {
                _dataPoints.RemoveAt(0);
            }
        }

        private void gridView_MouseWheel(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            var view = sender as GridView;
            view?.CloseEditor();
        }

        private void accessManager_ProjectReloaded(object sender, System.EventArgs e)
        {
            reloadTags();
        }

        private void gridViewTags_PopupMenuShowing(object sender, PopupMenuShowingEventArgs e)
        {
            if (e.Menu?.Items == null)
            {
                return;
            }

            if (e.HitInfo.InRow == false)
            {
                return;
            }

            int index = gridViewTags.GetFocusedDataSourceRowIndex();
            if (index >= _tagBindingSource.List.Count)
            {
                return;
            }

            var tag = _tagBindingSource.List[index] as Tag;

            switch (tag.Discriminator)
            {
                case Jankilla.Core.Tags.Base.ETagDiscriminator.Boolean:
                    _tagToRealtimeMenuItem.Enabled = true;
                    break;
                case Jankilla.Core.Tags.Base.ETagDiscriminator.Int:
                case Jankilla.Core.Tags.Base.ETagDiscriminator.Short:
                case Jankilla.Core.Tags.Base.ETagDiscriminator.Float:
                    _tagToRealtimeMenuItem.Enabled = true;
                    break;
                case Jankilla.Core.Tags.Base.ETagDiscriminator.String:
                    _tagToRealtimeMenuItem.Enabled = false;
                    break;
                default:
                    break;
            }

            e.Menu.Items.Add(_writeTagMenuItem);
            e.Menu.Items.Add(_tagToRealtimeMenuItem);
        }

        private void tagWriteMenuItem_Click(object sender, EventArgs e)
        {
            int index = gridViewTags.GetFocusedDataSourceRowIndex();
            if (index >= _tagBindingSource.List.Count)
            {
                return;
            }

            var item = _tagBindingSource.List[index] as Tag;

            if (item == null)
            {
                return;
            }

            string newValue = DialogHelper.ShowInputMessageBox($"[{item.Address}] 새로운 값을 써주세요. ");
            if (string.IsNullOrEmpty(newValue))
            {
                return;
            }

            switch (item.Discriminator)
            {
                case Jankilla.Core.Tags.Base.ETagDiscriminator.Boolean:
                    if (newValue == "0")
                    {
                        item.Write(false);
                        Trace.WriteLine($"[{item.Address}] [{item.Name}] [Write] [{false}]");
                    }
                    else
                    {
                        item.Write(true);
                        Trace.WriteLine($"[{item.Address}] [{item.Name}] [Write] [{true}]");
                    }
                    return;
                case Jankilla.Core.Tags.Base.ETagDiscriminator.Int:
                    bool bIntParsed = int.TryParse(newValue, out int newIntVal);
                    if (bIntParsed == false)
                    {
                        DialogHelper.ShowMessageBox($"[{newValue}]는 4바이트 정수로 변환 할 수 없습니다.");
                        return;
                    }

                    item.Write(newIntVal);
                    Trace.WriteLine($"[{item.Address}] [{item.Name}] [Write] [{newIntVal}]");
                    return;
                case Jankilla.Core.Tags.Base.ETagDiscriminator.Short:
                    bool bShortParsed = short.TryParse(newValue, out short newShortVal);
                    if (bShortParsed == false)
                    {
                        DialogHelper.ShowMessageBox($"[{newValue}]는 2바이트 정수로 변환 할 수 없습니다.");
                        return;
                    }

                    item.Write(newShortVal);
                    Trace.WriteLine($"[{item.Address}] [{item.Name}] [Write] [{newShortVal}]");
                    return;
                case Jankilla.Core.Tags.Base.ETagDiscriminator.String:
                    item.Write(newValue);
                    Trace.WriteLine($"[{item.Address}] [{item.Name}] [Write] [{newValue}]");
                    break;
                case Jankilla.Core.Tags.Base.ETagDiscriminator.Float:
                    bool bFloatParsed = float.TryParse(newValue, out float newFloatVal);
                    if (bFloatParsed == false)
                    {
                        DialogHelper.ShowMessageBox($"[{newValue}]는 4바이트 실수로 변환 할 수 없습니다.");
                        return;
                    }

                    item.Write(newFloatVal);
                    Trace.WriteLine($"[{item.Address}] [{item.Name}] [Write] [{newFloatVal}]");
                    return;
                default:
                    break;
            }
        }

        private void realtimeChartSetMenuItem_Click(object sender, EventArgs e)
        {
            int index = gridViewTags.GetFocusedDataSourceRowIndex();
            if (index >= _tagBindingSource.List.Count)
            {
                return;
            }

            var item = _tagBindingSource.List[index] as Tag;

            if (item == null)
            {
                return;
            }

            _dataPoints.Clear();

            _realtimeSelectedTag = item;
            layoutControlItemRealtimeTitle.Text = item.Name;

            Trace.WriteLine($"[{item.Address}] [{item.Name}] [REALTIME CHART SET]");
        }

        #endregion
    }

    internal class DataPoint
    {
        public DataPoint(DateTime argument, double value)
        {
            Argument = argument;
            Value = value;
        }
        public DateTime Argument { get; set; }
        public double Value { get; set; }
    }
}
