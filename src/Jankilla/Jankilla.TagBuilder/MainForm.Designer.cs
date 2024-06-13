
namespace Jankilla.TagBuilder
{
    partial class MainForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
            this.bar2 = new DevExpress.XtraBars.Bar();
            this.barSubItem1 = new DevExpress.XtraBars.BarSubItem();
            this.barButtonItemFileNew = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItemFileOpen = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItemFileSave = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItemCreateSample = new DevExpress.XtraBars.BarButtonItem();
            this.barSubItem2 = new DevExpress.XtraBars.BarSubItem();
            this.barSubItem4 = new DevExpress.XtraBars.BarSubItem();
            this.barButtonItem1 = new DevExpress.XtraBars.BarButtonItem();
            this.barSubItem3 = new DevExpress.XtraBars.BarSubItem();
            this.bar3 = new DevExpress.XtraBars.Bar();
            this.barStaticItemLicense = new DevExpress.XtraBars.BarStaticItem();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.treeListColumn1 = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
            this.gridControlTags = new DevExpress.XtraGrid.GridControl();
            this.tagBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.gridViewTags = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colNo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colDirection = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colByteSize = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colReadOnly = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colAddress = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colDescription = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colPath = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colDiscriminator = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colCategory = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colUnit = new DevExpress.XtraGrid.Columns.GridColumn();
            this.treeListChannels = new DevExpress.XtraTreeList.TreeList();
            this.treeListColumn2 = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.splitterItem1 = new DevExpress.XtraLayout.SplitterItem();
            this.layoutControlItemTags = new DevExpress.XtraLayout.LayoutControlItem();
            this.svgImageCollection1 = new DevExpress.Utils.SvgImageCollection(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControlTags)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tagBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewTags)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.treeListChannels)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitterItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemTags)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.svgImageCollection1)).BeginInit();
            this.SuspendLayout();
            // 
            // barManager1
            // 
            this.barManager1.AllowCustomization = false;
            this.barManager1.AllowQuickCustomization = false;
            this.barManager1.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.bar2,
            this.bar3});
            this.barManager1.DockControls.Add(this.barDockControlTop);
            this.barManager1.DockControls.Add(this.barDockControlBottom);
            this.barManager1.DockControls.Add(this.barDockControlLeft);
            this.barManager1.DockControls.Add(this.barDockControlRight);
            this.barManager1.Form = this;
            this.barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.barSubItem1,
            this.barSubItem2,
            this.barSubItem3,
            this.barSubItem4,
            this.barButtonItem1,
            this.barButtonItemFileNew,
            this.barButtonItemFileOpen,
            this.barButtonItemFileSave,
            this.barButtonItemCreateSample,
            this.barStaticItemLicense});
            this.barManager1.MainMenu = this.bar2;
            this.barManager1.MaxItemId = 14;
            this.barManager1.StatusBar = this.bar3;
            // 
            // bar2
            // 
            this.bar2.BarName = "Main menu";
            this.bar2.DockCol = 0;
            this.bar2.DockRow = 0;
            this.bar2.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.bar2.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.barSubItem1),
            new DevExpress.XtraBars.LinkPersistInfo(this.barSubItem2),
            new DevExpress.XtraBars.LinkPersistInfo(this.barSubItem3)});
            this.bar2.OptionsBar.MultiLine = true;
            this.bar2.OptionsBar.UseWholeRow = true;
            this.bar2.Text = "Main menu";
            // 
            // barSubItem1
            // 
            this.barSubItem1.Caption = "File";
            this.barSubItem1.Id = 0;
            this.barSubItem1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.KeyTip, this.barButtonItemFileNew, "", false, true, true, 0, null, DevExpress.XtraBars.BarItemPaintStyle.Standard, "CTRL+SHIFT+N", ""),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.KeyTip, this.barButtonItemFileOpen, "", false, true, true, 0, null, DevExpress.XtraBars.BarItemPaintStyle.Standard, "CTRL+SHIFT+O", ""),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItemFileSave),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItemCreateSample)});
            this.barSubItem1.Name = "barSubItem1";
            // 
            // barButtonItemFileNew
            // 
            this.barButtonItemFileNew.Caption = "&New";
            this.barButtonItemFileNew.Id = 8;
            this.barButtonItemFileNew.ItemInMenuAppearance.Normal.Options.UseTextOptions = true;
            this.barButtonItemFileNew.ItemInMenuAppearance.Normal.TextOptions.HotkeyPrefix = DevExpress.Utils.HKeyPrefix.Show;
            this.barButtonItemFileNew.ItemShortcut = new DevExpress.XtraBars.BarShortcut(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
                | System.Windows.Forms.Keys.N));
            this.barButtonItemFileNew.Name = "barButtonItemFileNew";
            this.barButtonItemFileNew.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItemFileNew_ItemClick);
            // 
            // barButtonItemFileOpen
            // 
            this.barButtonItemFileOpen.Caption = "&Open";
            this.barButtonItemFileOpen.Id = 9;
            this.barButtonItemFileOpen.ItemShortcut = new DevExpress.XtraBars.BarShortcut(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
                | System.Windows.Forms.Keys.O));
            this.barButtonItemFileOpen.Name = "barButtonItemFileOpen";
            this.barButtonItemFileOpen.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItemFileOpen_ItemClick);
            // 
            // barButtonItemFileSave
            // 
            this.barButtonItemFileSave.Caption = "&Save";
            this.barButtonItemFileSave.Id = 10;
            this.barButtonItemFileSave.ItemShortcut = new DevExpress.XtraBars.BarShortcut((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S));
            this.barButtonItemFileSave.Name = "barButtonItemFileSave";
            this.barButtonItemFileSave.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItemFileSave_ItemClick);
            // 
            // barButtonItemCreateSample
            // 
            this.barButtonItemCreateSample.Caption = "&Create Sample";
            this.barButtonItemCreateSample.Id = 11;
            this.barButtonItemCreateSample.Name = "barButtonItemCreateSample";
            this.barButtonItemCreateSample.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItemCreateSample_ItemClick);
            // 
            // barSubItem2
            // 
            this.barSubItem2.Caption = "Edit";
            this.barSubItem2.Id = 1;
            this.barSubItem2.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.barSubItem4)});
            this.barSubItem2.Name = "barSubItem2";
            // 
            // barSubItem4
            // 
            this.barSubItem4.Caption = "Tags";
            this.barSubItem4.Id = 6;
            this.barSubItem4.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem1)});
            this.barSubItem4.Name = "barSubItem4";
            // 
            // barButtonItem1
            // 
            this.barButtonItem1.Caption = "Import (.csv)";
            this.barButtonItem1.Id = 7;
            this.barButtonItem1.Name = "barButtonItem1";
            // 
            // barSubItem3
            // 
            this.barSubItem3.Caption = "View";
            this.barSubItem3.Id = 2;
            this.barSubItem3.Name = "barSubItem3";
            // 
            // bar3
            // 
            this.bar3.BarName = "Status bar";
            this.bar3.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Bottom;
            this.bar3.DockCol = 0;
            this.bar3.DockRow = 0;
            this.bar3.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom;
            this.bar3.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.barStaticItemLicense)});
            this.bar3.OptionsBar.AllowQuickCustomization = false;
            this.bar3.OptionsBar.DrawDragBorder = false;
            this.bar3.OptionsBar.UseWholeRow = true;
            this.bar3.Text = "Status bar";
            // 
            // barStaticItemLicense
            // 
            this.barStaticItemLicense.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
            this.barStaticItemLicense.Caption = "Copyright © J-ONESOFT 2024 All rights reserved.";
            this.barStaticItemLicense.Id = 13;
            this.barStaticItemLicense.ItemAppearance.Normal.Font = new System.Drawing.Font("Calibri", 7.5F);
            this.barStaticItemLicense.ItemAppearance.Normal.Options.UseFont = true;
            this.barStaticItemLicense.Name = "barStaticItemLicense";
            this.barStaticItemLicense.ItemDoubleClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barStaticItemLicense_ItemDoubleClick);
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Manager = this.barManager1;
            this.barDockControlTop.Size = new System.Drawing.Size(1482, 24);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 763);
            this.barDockControlBottom.Manager = this.barManager1;
            this.barDockControlBottom.Size = new System.Drawing.Size(1482, 26);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 24);
            this.barDockControlLeft.Manager = this.barManager1;
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 739);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(1482, 24);
            this.barDockControlRight.Manager = this.barManager1;
            this.barDockControlRight.Size = new System.Drawing.Size(0, 739);
            // 
            // treeListColumn1
            // 
            this.treeListColumn1.Name = "treeListColumn1";
            this.treeListColumn1.Visible = true;
            this.treeListColumn1.VisibleIndex = 0;
            // 
            // layoutControl1
            // 
            this.layoutControl1.Controls.Add(this.gridControlTags);
            this.layoutControl1.Controls.Add(this.treeListChannels);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl1.Location = new System.Drawing.Point(0, 24);
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.Root = this.Root;
            this.layoutControl1.Size = new System.Drawing.Size(1482, 739);
            this.layoutControl1.TabIndex = 9;
            this.layoutControl1.Text = "layoutControl1";
            // 
            // gridControlTags
            // 
            this.gridControlTags.DataSource = this.tagBindingSource;
            this.gridControlTags.Location = new System.Drawing.Point(400, 23);
            this.gridControlTags.MainView = this.gridViewTags;
            this.gridControlTags.MenuManager = this.barManager1;
            this.gridControlTags.Name = "gridControlTags";
            this.gridControlTags.Size = new System.Drawing.Size(1077, 711);
            this.gridControlTags.TabIndex = 5;
            this.gridControlTags.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridViewTags});
            // 
            // tagBindingSource
            // 
            this.tagBindingSource.DataSource = typeof(Jankilla.Core.Contracts.Tags.Tag);
            // 
            // gridViewTags
            // 
            this.gridViewTags.Appearance.FocusedRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.gridViewTags.Appearance.FocusedRow.Options.UseBackColor = true;
            this.gridViewTags.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colID,
            this.colNo,
            this.colName,
            this.colDirection,
            this.colByteSize,
            this.colReadOnly,
            this.colAddress,
            this.colDescription,
            this.colPath,
            this.colDiscriminator,
            this.colCategory,
            this.colUnit});
            this.gridViewTags.DetailHeight = 404;
            this.gridViewTags.GridControl = this.gridControlTags;
            this.gridViewTags.Name = "gridViewTags";
            this.gridViewTags.OptionsDetail.AllowExpandEmptyDetails = true;
            this.gridViewTags.OptionsView.ShowGroupPanel = false;
            this.gridViewTags.PopupMenuShowing += new DevExpress.XtraGrid.Views.Grid.PopupMenuShowingEventHandler(this.gridViewTags_PopupMenuShowing);
            // 
            // colID
            // 
            this.colID.FieldName = "ID";
            this.colID.MinWidth = 23;
            this.colID.Name = "colID";
            this.colID.OptionsColumn.ReadOnly = true;
            this.colID.Width = 68;
            // 
            // colNo
            // 
            this.colNo.FieldName = "No";
            this.colNo.MinWidth = 23;
            this.colNo.Name = "colNo";
            this.colNo.OptionsColumn.ReadOnly = true;
            this.colNo.Visible = true;
            this.colNo.VisibleIndex = 0;
            this.colNo.Width = 71;
            // 
            // colName
            // 
            this.colName.FieldName = "Name";
            this.colName.MinWidth = 23;
            this.colName.Name = "colName";
            this.colName.Visible = true;
            this.colName.VisibleIndex = 3;
            this.colName.Width = 272;
            // 
            // colDirection
            // 
            this.colDirection.FieldName = "Direction";
            this.colDirection.MinWidth = 23;
            this.colDirection.Name = "colDirection";
            this.colDirection.Visible = true;
            this.colDirection.VisibleIndex = 4;
            this.colDirection.Width = 86;
            // 
            // colByteSize
            // 
            this.colByteSize.FieldName = "ByteSize";
            this.colByteSize.MinWidth = 23;
            this.colByteSize.Name = "colByteSize";
            this.colByteSize.OptionsColumn.ReadOnly = true;
            this.colByteSize.Visible = true;
            this.colByteSize.VisibleIndex = 5;
            this.colByteSize.Width = 80;
            // 
            // colReadOnly
            // 
            this.colReadOnly.FieldName = "ReadOnly";
            this.colReadOnly.MinWidth = 23;
            this.colReadOnly.Name = "colReadOnly";
            this.colReadOnly.Width = 70;
            // 
            // colAddress
            // 
            this.colAddress.FieldName = "Address";
            this.colAddress.MinWidth = 23;
            this.colAddress.Name = "colAddress";
            this.colAddress.OptionsColumn.ReadOnly = true;
            this.colAddress.Visible = true;
            this.colAddress.VisibleIndex = 6;
            this.colAddress.Width = 125;
            // 
            // colDescription
            // 
            this.colDescription.FieldName = "Description";
            this.colDescription.MinWidth = 23;
            this.colDescription.Name = "colDescription";
            this.colDescription.Visible = true;
            this.colDescription.VisibleIndex = 8;
            this.colDescription.Width = 152;
            // 
            // colPath
            // 
            this.colPath.FieldName = "Path";
            this.colPath.MinWidth = 23;
            this.colPath.Name = "colPath";
            this.colPath.Width = 70;
            // 
            // colDiscriminator
            // 
            this.colDiscriminator.Caption = "Type";
            this.colDiscriminator.FieldName = "Discriminator";
            this.colDiscriminator.MinWidth = 23;
            this.colDiscriminator.Name = "colDiscriminator";
            this.colDiscriminator.OptionsColumn.ReadOnly = true;
            this.colDiscriminator.Visible = true;
            this.colDiscriminator.VisibleIndex = 2;
            this.colDiscriminator.Width = 80;
            // 
            // colCategory
            // 
            this.colCategory.FieldName = "Category";
            this.colCategory.MinWidth = 23;
            this.colCategory.Name = "colCategory";
            this.colCategory.Visible = true;
            this.colCategory.VisibleIndex = 1;
            this.colCategory.Width = 119;
            // 
            // colUnit
            // 
            this.colUnit.Caption = "Unit";
            this.colUnit.FieldName = "Unit";
            this.colUnit.Name = "colUnit";
            this.colUnit.OptionsColumn.ReadOnly = true;
            this.colUnit.Visible = true;
            this.colUnit.VisibleIndex = 7;
            // 
            // treeListChannels
            // 
            this.treeListChannels.Appearance.FocusedCell.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.treeListChannels.Appearance.FocusedCell.Options.UseBackColor = true;
            this.treeListChannels.Appearance.FocusedRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.treeListChannels.Appearance.FocusedRow.Options.UseBackColor = true;
            this.treeListChannels.Appearance.SelectedRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.treeListChannels.Appearance.SelectedRow.Options.UseBackColor = true;
            this.treeListChannels.Columns.AddRange(new DevExpress.XtraTreeList.Columns.TreeListColumn[] {
            this.treeListColumn2});
            this.treeListChannels.Location = new System.Drawing.Point(5, 23);
            this.treeListChannels.MenuManager = this.barManager1;
            this.treeListChannels.MinWidth = 23;
            this.treeListChannels.Name = "treeListChannels";
            this.treeListChannels.OptionsBehavior.ReadOnly = true;
            this.treeListChannels.OptionsSelection.MultiSelect = true;
            this.treeListChannels.OptionsSelection.MultiSelectMode = DevExpress.XtraTreeList.TreeListMultiSelectMode.CellSelect;
            this.treeListChannels.OptionsView.ShowTreeLines = DevExpress.Utils.DefaultBoolean.True;
            this.treeListChannels.Size = new System.Drawing.Size(386, 711);
            this.treeListChannels.TabIndex = 4;
            this.treeListChannels.TreeLevelWidth = 21;
            this.treeListChannels.ViewStyle = DevExpress.XtraTreeList.TreeListViewStyle.TreeView;
            this.treeListChannels.FocusedNodeChanged += new DevExpress.XtraTreeList.FocusedNodeChangedEventHandler(this.treeListChannels_FocusedNodeChanged);
            this.treeListChannels.PopupMenuShowing += new DevExpress.XtraTreeList.PopupMenuShowingEventHandler(this.treeListChannels_PopupMenuShowing);
            // 
            // treeListColumn2
            // 
            this.treeListColumn2.MinWidth = 23;
            this.treeListColumn2.Name = "treeListColumn2";
            this.treeListColumn2.Visible = true;
            this.treeListColumn2.VisibleIndex = 0;
            this.treeListColumn2.Width = 87;
            // 
            // Root
            // 
            this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.Root.GroupBordersVisible = false;
            this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1,
            this.splitterItem1,
            this.layoutControlItemTags});
            this.Root.Name = "Root";
            this.Root.Padding = new DevExpress.XtraLayout.Utils.Padding(3, 3, 3, 3);
            this.Root.Size = new System.Drawing.Size(1482, 739);
            this.Root.TextVisible = false;
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.treeListChannels;
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(390, 733);
            this.layoutControlItem1.Text = "✅   Channels";
            this.layoutControlItem1.TextLocation = DevExpress.Utils.Locations.Top;
            this.layoutControlItem1.TextSize = new System.Drawing.Size(67, 15);
            // 
            // splitterItem1
            // 
            this.splitterItem1.AllowHotTrack = true;
            this.splitterItem1.Location = new System.Drawing.Point(390, 0);
            this.splitterItem1.Name = "splitterItem1";
            this.splitterItem1.Size = new System.Drawing.Size(5, 733);
            // 
            // layoutControlItemTags
            // 
            this.layoutControlItemTags.Control = this.gridControlTags;
            this.layoutControlItemTags.Location = new System.Drawing.Point(395, 0);
            this.layoutControlItemTags.Name = "layoutControlItemTags";
            this.layoutControlItemTags.Size = new System.Drawing.Size(1081, 733);
            this.layoutControlItemTags.Text = "💿  Tags";
            this.layoutControlItemTags.TextLocation = DevExpress.Utils.Locations.Top;
            this.layoutControlItemTags.TextSize = new System.Drawing.Size(67, 15);
            // 
            // svgImageCollection1
            // 
            this.svgImageCollection1.Add("actions_add", "image://svgimages/icon builder/actions_add.svg");
            this.svgImageCollection1.Add("actions_deletecircled", "image://svgimages/icon builder/actions_deletecircled.svg");
            this.svgImageCollection1.Add("edit", "image://svgimages/dashboards/edit.svg");
            this.svgImageCollection1.Add("copy", "image://svgimages/pdf viewer/copy.svg");
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1482, 789);
            this.Controls.Add(this.layoutControl1);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.IconOptions.Icon = ((System.Drawing.Icon)(resources.GetObject("MainForm.IconOptions.Icon")));
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Tag Builder";
            this.Load += new System.EventHandler(this.mainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            this.layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControlTags)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tagBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewTags)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.treeListChannels)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitterItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemTags)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.svgImageCollection1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraBars.BarManager barManager1;
        private DevExpress.XtraBars.Bar bar2;
        private DevExpress.XtraBars.Bar bar3;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraBars.BarSubItem barSubItem1;
        private DevExpress.XtraBars.BarSubItem barSubItem2;
        private DevExpress.XtraBars.BarSubItem barSubItem3;
        private DevExpress.XtraTreeList.Columns.TreeListColumn treeListColumn1;
        private DevExpress.XtraLayout.LayoutControl layoutControl1;
        private DevExpress.XtraLayout.LayoutControlGroup Root;
        private DevExpress.XtraGrid.GridControl gridControlTags;
        private DevExpress.XtraGrid.Views.Grid.GridView gridViewTags;
        private DevExpress.XtraTreeList.TreeList treeListChannels;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraLayout.SplitterItem splitterItem1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItemTags;
        private DevExpress.XtraTreeList.Columns.TreeListColumn treeListColumn2;
        private DevExpress.XtraBars.BarButtonItem barButtonItemFileNew;
        private DevExpress.XtraBars.BarSubItem barSubItem4;
        private DevExpress.XtraBars.BarButtonItem barButtonItem1;
        private DevExpress.XtraBars.BarButtonItem barButtonItemFileOpen;
        private DevExpress.Utils.SvgImageCollection svgImageCollection1;
        private DevExpress.XtraBars.BarButtonItem barButtonItemFileSave;
        private System.Windows.Forms.BindingSource tagBindingSource;
        private DevExpress.XtraGrid.Columns.GridColumn colID;
        private DevExpress.XtraGrid.Columns.GridColumn colNo;
        private DevExpress.XtraGrid.Columns.GridColumn colName;
        private DevExpress.XtraGrid.Columns.GridColumn colDirection;
        private DevExpress.XtraGrid.Columns.GridColumn colByteSize;
        private DevExpress.XtraGrid.Columns.GridColumn colReadOnly;
        private DevExpress.XtraGrid.Columns.GridColumn colAddress;
        private DevExpress.XtraGrid.Columns.GridColumn colDescription;
        private DevExpress.XtraGrid.Columns.GridColumn colPath;
        private DevExpress.XtraGrid.Columns.GridColumn colDiscriminator;
        private DevExpress.XtraGrid.Columns.GridColumn colCategory;
        private DevExpress.XtraBars.BarButtonItem barButtonItemCreateSample;
        private DevExpress.XtraGrid.Columns.GridColumn colUnit;
        private DevExpress.XtraBars.BarStaticItem barStaticItemLicense;
    }
}