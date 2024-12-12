namespace Jankilla.Sample.WinForms.Controls.Pages
{
    partial class MainHomePageUserControl
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
            this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
            this.gridControlTags = new DevExpress.XtraGrid.GridControl();
            this.gridViewTags = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colNo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colDirection = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colByteSize = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colAddress = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colCategory = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colDescription = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colValue = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colCalibratedValue = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colUnit = new DevExpress.XtraGrid.Columns.GridColumn();
            this.chartControlRealtime = new DevExpress.XtraCharts.ChartControl();
            this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlGroup2 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItemRealtimeTitle = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControlTags)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewTags)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartControlRealtime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemRealtimeTitle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            this.SuspendLayout();
            // 
            // layoutControl1
            // 
            this.layoutControl1.AllowCustomization = true;
            this.layoutControl1.Controls.Add(this.gridControlTags);
            this.layoutControl1.Controls.Add(this.chartControlRealtime);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl1.Location = new System.Drawing.Point(0, 0);
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.Root = this.Root;
            this.layoutControl1.Size = new System.Drawing.Size(150, 150);
            this.layoutControl1.TabIndex = 1;
            this.layoutControl1.Text = "layoutControl1";
            // 
            // gridControlTags
            // 
            this.gridControlTags.Location = new System.Drawing.Point(14, 87);
            this.gridControlTags.MainView = this.gridViewTags;
            this.gridControlTags.Name = "gridControlTags";
            this.gridControlTags.Size = new System.Drawing.Size(122, 49);
            this.gridControlTags.TabIndex = 5;
            this.gridControlTags.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridViewTags});
            // 
            // gridViewTags
            // 
            this.gridViewTags.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colNo,
            this.colName,
            this.colDirection,
            this.colByteSize,
            this.colAddress,
            this.colCategory,
            this.colDescription,
            this.colValue,
            this.colCalibratedValue,
            this.colUnit});
            this.gridViewTags.DetailHeight = 404;
            this.gridViewTags.GridControl = this.gridControlTags;
            this.gridViewTags.Name = "gridViewTags";
            this.gridViewTags.OptionsBehavior.Editable = false;
            this.gridViewTags.OptionsBehavior.ReadOnly = true;
            this.gridViewTags.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridViewTags.OptionsView.ShowGroupPanel = false;
            this.gridViewTags.SortInfo.AddRange(new DevExpress.XtraGrid.Columns.GridColumnSortInfo[] {
            new DevExpress.XtraGrid.Columns.GridColumnSortInfo(this.colNo, DevExpress.Data.ColumnSortOrder.Ascending)});
            // 
            // colNo
            // 
            this.colNo.FieldName = "No";
            this.colNo.Name = "colNo";
            this.colNo.Visible = true;
            this.colNo.VisibleIndex = 0;
            this.colNo.Width = 47;
            // 
            // colName
            // 
            this.colName.FieldName = "Name";
            this.colName.Name = "colName";
            this.colName.Visible = true;
            this.colName.VisibleIndex = 2;
            this.colName.Width = 168;
            // 
            // colDirection
            // 
            this.colDirection.FieldName = "Direction";
            this.colDirection.Name = "colDirection";
            this.colDirection.Visible = true;
            this.colDirection.VisibleIndex = 3;
            this.colDirection.Width = 76;
            // 
            // colByteSize
            // 
            this.colByteSize.FieldName = "ByteSize";
            this.colByteSize.Name = "colByteSize";
            this.colByteSize.OptionsColumn.ReadOnly = true;
            this.colByteSize.Visible = true;
            this.colByteSize.VisibleIndex = 4;
            this.colByteSize.Width = 78;
            // 
            // colAddress
            // 
            this.colAddress.FieldName = "Address";
            this.colAddress.Name = "colAddress";
            this.colAddress.Visible = true;
            this.colAddress.VisibleIndex = 6;
            this.colAddress.Width = 133;
            // 
            // colCategory
            // 
            this.colCategory.FieldName = "Category";
            this.colCategory.Name = "colCategory";
            this.colCategory.Visible = true;
            this.colCategory.VisibleIndex = 1;
            this.colCategory.Width = 148;
            // 
            // colDescription
            // 
            this.colDescription.FieldName = "Description";
            this.colDescription.Name = "colDescription";
            this.colDescription.Visible = true;
            this.colDescription.VisibleIndex = 5;
            this.colDescription.Width = 102;
            // 
            // colValue
            // 
            this.colValue.FieldName = "Value";
            this.colValue.Name = "colValue";
            this.colValue.OptionsColumn.ReadOnly = true;
            this.colValue.Width = 162;
            // 
            // colCalibratedValue
            // 
            this.colCalibratedValue.FieldName = "CalibratedValue";
            this.colCalibratedValue.Name = "colCalibratedValue";
            this.colCalibratedValue.OptionsColumn.ReadOnly = true;
            this.colCalibratedValue.OptionsFilter.AllowFilter = false;
            this.colCalibratedValue.Visible = true;
            this.colCalibratedValue.VisibleIndex = 7;
            this.colCalibratedValue.Width = 155;
            // 
            // colUnit
            // 
            this.colUnit.FieldName = "Unit";
            this.colUnit.Name = "colUnit";
            this.colUnit.Visible = true;
            this.colUnit.VisibleIndex = 8;
            this.colUnit.Width = 177;
            // 
            // chartControlRealtime
            // 
            this.chartControlRealtime.Legend.Name = "Default Legend";
            this.chartControlRealtime.Location = new System.Drawing.Point(14, 57);
            this.chartControlRealtime.Name = "chartControlRealtime";
            this.chartControlRealtime.PaletteName = "Apex";
            this.chartControlRealtime.SeriesSerializable = new DevExpress.XtraCharts.Series[0];
            this.chartControlRealtime.Size = new System.Drawing.Size(122, 26);
            this.chartControlRealtime.TabIndex = 4;
            // 
            // Root
            // 
            this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.Root.GroupBordersVisible = false;
            this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlGroup2});
            this.Root.Name = "Root";
            this.Root.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.Root.Size = new System.Drawing.Size(150, 150);
            this.Root.TextVisible = false;
            // 
            // layoutControlGroup2
            // 
            this.layoutControlGroup2.GroupStyle = DevExpress.Utils.GroupStyle.Light;
            this.layoutControlGroup2.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItemRealtimeTitle,
            this.layoutControlItem2});
            this.layoutControlGroup2.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup2.Name = "layoutControlGroup2";
            this.layoutControlGroup2.Size = new System.Drawing.Size(150, 150);
            this.layoutControlGroup2.Text = "INFORMATION";
            // 
            // layoutControlItemRealtimeTitle
            // 
            this.layoutControlItemRealtimeTitle.Control = this.chartControlRealtime;
            this.layoutControlItemRealtimeTitle.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItemRealtimeTitle.Name = "layoutControlItemRealtimeTitle";
            this.layoutControlItemRealtimeTitle.Size = new System.Drawing.Size(126, 48);
            this.layoutControlItemRealtimeTitle.Text = "REALTIME CHART";
            this.layoutControlItemRealtimeTitle.TextLocation = DevExpress.Utils.Locations.Top;
            this.layoutControlItemRealtimeTitle.TextSize = new System.Drawing.Size(91, 15);
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this.gridControlTags;
            this.layoutControlItem2.Location = new System.Drawing.Point(0, 48);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Size = new System.Drawing.Size(126, 53);
            this.layoutControlItem2.Text = "REALTIME TABLE";
            this.layoutControlItem2.TextLocation = DevExpress.Utils.Locations.Top;
            this.layoutControlItem2.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem2.TextVisible = false;
            // 
            // XtraUserControl1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.layoutControl1);
            this.Name = "XtraUserControl1";
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            this.layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControlTags)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewTags)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartControlRealtime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemRealtimeTitle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraLayout.LayoutControl layoutControl1;
        private DevExpress.XtraGrid.GridControl gridControlTags;
        private DevExpress.XtraGrid.Views.Grid.GridView gridViewTags;
        private DevExpress.XtraGrid.Columns.GridColumn colNo;
        private DevExpress.XtraGrid.Columns.GridColumn colName;
        private DevExpress.XtraGrid.Columns.GridColumn colDirection;
        private DevExpress.XtraGrid.Columns.GridColumn colByteSize;
        private DevExpress.XtraGrid.Columns.GridColumn colAddress;
        private DevExpress.XtraGrid.Columns.GridColumn colCategory;
        private DevExpress.XtraGrid.Columns.GridColumn colDescription;
        private DevExpress.XtraGrid.Columns.GridColumn colValue;
        private DevExpress.XtraGrid.Columns.GridColumn colCalibratedValue;
        private DevExpress.XtraGrid.Columns.GridColumn colUnit;
        private DevExpress.XtraCharts.ChartControl chartControlRealtime;
        private DevExpress.XtraLayout.LayoutControlGroup Root;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup2;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItemRealtimeTitle;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
    }
}
