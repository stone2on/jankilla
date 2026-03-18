namespace Jankilla.Sample.WinForms.Controls.Pages
{
    partial class HistorySearchPageControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
            this.listBoxControlCategories = new DevExpress.XtraEditors.ListBoxControl();
            this.chartControlTagValue = new DevExpress.XtraCharts.ChartControl();
            this.gridControlTagValue = new DevExpress.XtraGrid.GridControl();
            this.gridViewTagValue = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.repositoryItemDateEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemDateEdit();
            this.comboBoxEditDateSearchOption = new DevExpress.XtraEditors.ComboBoxEdit();
            this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItemChart = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem4 = new DevExpress.XtraLayout.LayoutControlItem();
            this.simpleSeparator1 = new DevExpress.XtraLayout.SimpleSeparator();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.simpleLabelItemDataTable = new DevExpress.XtraLayout.SimpleLabelItem();
            this.simpleLabelItem1 = new DevExpress.XtraLayout.SimpleLabelItem();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.simpleLabelItemUpdatedTime = new DevExpress.XtraLayout.SimpleLabelItem();
            this.simpleSeparator2 = new DevExpress.XtraLayout.SimpleSeparator();
            this.popupMenuExport = new DevExpress.XtraBars.PopupMenu(this.components);
            this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.barButtonItemExportToCsv = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItemExportToXlsx = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItemExportToPdf = new DevExpress.XtraBars.BarButtonItem();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.listBoxControlCategories)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartControlTagValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControlTagValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewTagValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit1.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxEditDateSearchOption.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemChart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.simpleSeparator1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.simpleLabelItemDataTable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.simpleLabelItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.simpleLabelItemUpdatedTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.simpleSeparator2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupMenuExport)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            this.SuspendLayout();
            // 
            // layoutControl1
            // 
            this.layoutControl1.AllowCustomization = true;
            this.layoutControl1.Controls.Add(this.listBoxControlCategories);
            this.layoutControl1.Controls.Add(this.chartControlTagValue);
            this.layoutControl1.Controls.Add(this.gridControlTagValue);
            this.layoutControl1.Controls.Add(this.comboBoxEditDateSearchOption);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl1.Location = new System.Drawing.Point(0, 0);
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.Root = this.Root;
            this.layoutControl1.Size = new System.Drawing.Size(150, 150);
            this.layoutControl1.TabIndex = 2;
            this.layoutControl1.Text = "layoutControl1";
            // 
            // listBoxControlCategories
            // 
            this.listBoxControlCategories.Location = new System.Drawing.Point(2, 21);
            this.listBoxControlCategories.Name = "listBoxControlCategories";
            this.listBoxControlCategories.Size = new System.Drawing.Size(212, 0);
            this.listBoxControlCategories.StyleController = this.layoutControl1;
            this.listBoxControlCategories.TabIndex = 9;
            // 
            // chartControlTagValue
            // 
            this.chartControlTagValue.Legend.Name = "Default Legend";
            this.chartControlTagValue.Location = new System.Drawing.Point(2, 89);
            this.chartControlTagValue.Name = "chartControlTagValue";
            this.chartControlTagValue.SeriesSerializable = new DevExpress.XtraCharts.Series[0];
            this.chartControlTagValue.Size = new System.Drawing.Size(365, 44);
            this.chartControlTagValue.TabIndex = 6;
            // 
            // gridControlTagValue
            // 
            this.gridControlTagValue.Location = new System.Drawing.Point(219, 21);
            this.gridControlTagValue.MainView = this.gridViewTagValue;
            this.gridControlTagValue.Name = "gridControlTagValue";
            this.gridControlTagValue.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemDateEdit1});
            this.gridControlTagValue.Size = new System.Drawing.Size(148, 46);
            this.gridControlTagValue.TabIndex = 5;
            this.gridControlTagValue.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridViewTagValue});
            // 
            // gridViewTagValue
            // 
            this.gridViewTagValue.DetailHeight = 404;
            this.gridViewTagValue.GridControl = this.gridControlTagValue;
            this.gridViewTagValue.Name = "gridViewTagValue";
            this.gridViewTagValue.OptionsBehavior.Editable = false;
            this.gridViewTagValue.OptionsBehavior.ReadOnly = true;
            this.gridViewTagValue.OptionsPrint.AutoWidth = false;
            this.gridViewTagValue.OptionsView.ColumnAutoWidth = false;
            this.gridViewTagValue.OptionsView.ShowGroupPanel = false;
            // 
            // repositoryItemDateEdit1
            // 
            this.repositoryItemDateEdit1.AutoHeight = false;
            this.repositoryItemDateEdit1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemDateEdit1.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemDateEdit1.DisplayFormat.FormatString = "yyyy-MM-dd tt hh:mm:ss.fff";
            this.repositoryItemDateEdit1.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.repositoryItemDateEdit1.MaskSettings.Set("mask", "G");
            this.repositoryItemDateEdit1.Name = "repositoryItemDateEdit1";
            this.repositoryItemDateEdit1.ReadOnly = true;
            this.repositoryItemDateEdit1.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            // 
            // comboBoxEditDateSearchOption
            // 
            this.comboBoxEditDateSearchOption.Location = new System.Drawing.Point(162, 25);
            this.comboBoxEditDateSearchOption.Name = "comboBoxEditDateSearchOption";
            this.comboBoxEditDateSearchOption.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.comboBoxEditDateSearchOption.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            
            this.comboBoxEditDateSearchOption.Size = new System.Drawing.Size(52, 22);
            this.comboBoxEditDateSearchOption.StyleController = this.layoutControl1;
            this.comboBoxEditDateSearchOption.TabIndex = 8;
            // 
            // Root
            // 
            this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.Root.GroupBordersVisible = false;
            this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItemChart,
            this.layoutControlItem4,
            this.simpleSeparator1,
            this.layoutControlItem2,
            this.simpleLabelItemDataTable,
            this.simpleLabelItem1,
            this.layoutControlItem1,
            this.simpleLabelItemUpdatedTime,
            this.simpleSeparator2});
            this.Root.Name = "Root";
            this.Root.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.Root.Size = new System.Drawing.Size(369, 133);
            this.Root.TextVisible = false;
            // 
            // layoutControlItemChart
            // 
            this.layoutControlItemChart.Control = this.chartControlTagValue;
            this.layoutControlItemChart.ImageOptions.SvgImageSize = new System.Drawing.Size(12, 12);
            this.layoutControlItemChart.Location = new System.Drawing.Point(0, 69);
            this.layoutControlItemChart.Name = "layoutControlItemChart";
            this.layoutControlItemChart.OptionsPrint.AppearanceItem.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Italic);
            this.layoutControlItemChart.OptionsPrint.AppearanceItem.Options.UseFont = true;
            this.layoutControlItemChart.Padding = new DevExpress.XtraLayout.Utils.Padding(2, 2, 2, 0);
            this.layoutControlItemChart.Size = new System.Drawing.Size(369, 64);
            this.layoutControlItemChart.Text = "Chart View";
            this.layoutControlItemChart.TextLocation = DevExpress.Utils.Locations.Top;
            this.layoutControlItemChart.TextSize = new System.Drawing.Size(148, 15);
            // 
            // layoutControlItem4
            // 
            this.layoutControlItem4.Control = this.listBoxControlCategories;
            this.layoutControlItem4.ImageOptions.SvgImageSize = new System.Drawing.Size(12, 12);
            this.layoutControlItem4.Location = new System.Drawing.Point(0, 19);
            this.layoutControlItem4.Name = "layoutControlItem4";
            this.layoutControlItem4.Size = new System.Drawing.Size(216, 4);
            this.layoutControlItem4.Text = "Categories";
            this.layoutControlItem4.TextLocation = DevExpress.Utils.Locations.Top;
            this.layoutControlItem4.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem4.TextVisible = false;
            // 
            // simpleSeparator1
            // 
            this.simpleSeparator1.AllowHotTrack = false;
            this.simpleSeparator1.Location = new System.Drawing.Point(216, 0);
            this.simpleSeparator1.Name = "simpleSeparator1";
            this.simpleSeparator1.Size = new System.Drawing.Size(1, 69);
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this.gridControlTagValue;
            this.layoutControlItem2.ImageOptions.SvgImageSize = new System.Drawing.Size(12, 12);
            this.layoutControlItem2.Location = new System.Drawing.Point(217, 19);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Size = new System.Drawing.Size(152, 50);
            this.layoutControlItem2.Text = "Data Table";
            this.layoutControlItem2.TextLocation = DevExpress.Utils.Locations.Top;
            this.layoutControlItem2.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem2.TextVisible = false;
            // 
            // simpleLabelItemDataTable
            // 
            this.simpleLabelItemDataTable.AllowHotTrack = false;
            this.simpleLabelItemDataTable.ImageOptions.SvgImageSize = new System.Drawing.Size(12, 12);
            this.simpleLabelItemDataTable.Location = new System.Drawing.Point(217, 0);
            this.simpleLabelItemDataTable.Name = "simpleLabelItemDataTable";
            this.simpleLabelItemDataTable.OptionsToolTip.ToolTip = "Right-click to show a menu that allows you to extract data.";
            this.simpleLabelItemDataTable.OptionsToolTip.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Information;
            this.simpleLabelItemDataTable.Padding = new DevExpress.XtraLayout.Utils.Padding(2, 2, 4, 0);
            this.simpleLabelItemDataTable.Size = new System.Drawing.Size(152, 19);
            this.simpleLabelItemDataTable.Text = "Data Table";
            this.simpleLabelItemDataTable.TextSize = new System.Drawing.Size(148, 15);
            // 
            // simpleLabelItem1
            // 
            this.simpleLabelItem1.AllowHotTrack = false;
            this.simpleLabelItem1.ImageOptions.SvgImageSize = new System.Drawing.Size(12, 12);
            this.simpleLabelItem1.Location = new System.Drawing.Point(0, 0);
            this.simpleLabelItem1.Name = "simpleLabelItem1";
            this.simpleLabelItem1.Padding = new DevExpress.XtraLayout.Utils.Padding(2, 2, 4, 0);
            this.simpleLabelItem1.Size = new System.Drawing.Size(216, 19);
            this.simpleLabelItem1.Text = "Categories";
            this.simpleLabelItem1.TextSize = new System.Drawing.Size(148, 15);
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.comboBoxEditDateSearchOption;
            this.layoutControlItem1.ImageOptions.SvgImageSize = new System.Drawing.Size(12, 12);
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 23);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(216, 26);
            this.layoutControlItem1.Text = "Search Option";
            this.layoutControlItem1.TextLocation = DevExpress.Utils.Locations.Left;
            this.layoutControlItem1.TextSize = new System.Drawing.Size(148, 15);
            // 
            // simpleLabelItemUpdatedTime
            // 
            this.simpleLabelItemUpdatedTime.AllowHotTrack = false;
            this.simpleLabelItemUpdatedTime.AppearanceItemCaption.Font = new System.Drawing.Font("Calibri", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))));
            this.simpleLabelItemUpdatedTime.AppearanceItemCaption.Options.UseFont = true;
            this.simpleLabelItemUpdatedTime.AppearanceItemCaption.Options.UseTextOptions = true;
            this.simpleLabelItemUpdatedTime.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.simpleLabelItemUpdatedTime.Location = new System.Drawing.Point(0, 49);
            this.simpleLabelItemUpdatedTime.Name = "simpleLabelItemUpdatedTime";
            this.simpleLabelItemUpdatedTime.OptionsPrint.AppearanceItem.Font = new System.Drawing.Font("Calibri", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))));
            this.simpleLabelItemUpdatedTime.OptionsPrint.AppearanceItem.Options.UseFont = true;
            this.simpleLabelItemUpdatedTime.Size = new System.Drawing.Size(216, 19);
            this.simpleLabelItemUpdatedTime.Text = "* 0000-00-00 AM 00:00:00";
            this.simpleLabelItemUpdatedTime.TextSize = new System.Drawing.Size(148, 15);
            // 
            // simpleSeparator2
            // 
            this.simpleSeparator2.AllowHotTrack = false;
            this.simpleSeparator2.Location = new System.Drawing.Point(0, 68);
            this.simpleSeparator2.Name = "simpleSeparator2";
            this.simpleSeparator2.Size = new System.Drawing.Size(216, 1);
            // 
            // popupMenuExport
            // 
            this.popupMenuExport.Name = "popupMenuExport";
            // 
            // barManager1
            // 
            this.barManager1.DockControls.Add(this.barDockControlTop);
            this.barManager1.DockControls.Add(this.barDockControlBottom);
            this.barManager1.DockControls.Add(this.barDockControlLeft);
            this.barManager1.DockControls.Add(this.barDockControlRight);
            this.barManager1.Form = this;
            this.barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.barButtonItemExportToCsv,
            this.barButtonItemExportToXlsx,
            this.barButtonItemExportToPdf});
            this.barManager1.MaxItemId = 3;
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Manager = this.barManager1;
            this.barDockControlTop.Size = new System.Drawing.Size(150, 0);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 150);
            this.barDockControlBottom.Manager = this.barManager1;
            this.barDockControlBottom.Size = new System.Drawing.Size(150, 0);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 0);
            this.barDockControlLeft.Manager = this.barManager1;
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 150);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(150, 0);
            this.barDockControlRight.Manager = this.barManager1;
            this.barDockControlRight.Size = new System.Drawing.Size(0, 150);
            // 
            // barButtonItemExportToCsv
            // 
            this.barButtonItemExportToCsv.Caption = "Export to (.csv)";
            this.barButtonItemExportToCsv.Id = 0;
            this.barButtonItemExportToCsv.Name = "barButtonItemExportToCsv";
            // 
            // barButtonItemExportToXlsx
            // 
            this.barButtonItemExportToXlsx.Caption = "Export to (.xlsx)";
            this.barButtonItemExportToXlsx.Id = 1;
            this.barButtonItemExportToXlsx.Name = "barButtonItemExportToXlsx";
            // 
            // barButtonItemExportToPdf
            // 
            this.barButtonItemExportToPdf.Caption = "Export to (.pdf)";
            this.barButtonItemExportToPdf.Id = 2;
            this.barButtonItemExportToPdf.Name = "barButtonItemExportToPdf";
            // 
            // HistorySearchPageControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Controls.Add(this.layoutControl1);
            this.Name = "HistorySearchPageControl";
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            this.layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.listBoxControlCategories)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartControlTagValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControlTagValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewTagValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit1.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxEditDateSearchOption.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemChart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.simpleSeparator1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.simpleLabelItemDataTable)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.simpleLabelItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.simpleLabelItemUpdatedTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.simpleSeparator2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupMenuExport)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraLayout.LayoutControl layoutControl1;
        private DevExpress.XtraEditors.ListBoxControl listBoxControlCategories;
        private DevExpress.XtraCharts.ChartControl chartControlTagValue;
        private DevExpress.XtraGrid.GridControl gridControlTagValue;
        private DevExpress.XtraGrid.Views.Grid.GridView gridViewTagValue;
        private DevExpress.XtraEditors.Repository.RepositoryItemDateEdit repositoryItemDateEdit1;
        private DevExpress.XtraEditors.ComboBoxEdit comboBoxEditDateSearchOption;
        private DevExpress.XtraLayout.LayoutControlGroup Root;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItemChart;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem4;
        private DevExpress.XtraLayout.SimpleSeparator simpleSeparator1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
        private DevExpress.XtraLayout.SimpleLabelItem simpleLabelItemDataTable;
        private DevExpress.XtraLayout.SimpleLabelItem simpleLabelItem1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraLayout.SimpleLabelItem simpleLabelItemUpdatedTime;
        private DevExpress.XtraLayout.SimpleSeparator simpleSeparator2;
        private DevExpress.XtraBars.PopupMenu popupMenuExport;
        private DevExpress.XtraBars.BarManager barManager1;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraBars.BarButtonItem barButtonItemExportToCsv;
        private DevExpress.XtraBars.BarButtonItem barButtonItemExportToXlsx;
        private DevExpress.XtraBars.BarButtonItem barButtonItemExportToPdf;
    }
}
