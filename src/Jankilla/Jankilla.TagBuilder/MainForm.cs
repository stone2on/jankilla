using DevExpress.Utils.Menu;
using DevExpress.XtraBars;
using DevExpress.XtraSplashScreen;
using Jankilla.Core.Contracts;
using Jankilla.Core.Contracts.Tags;
using Jankilla.Core.Contracts.Tags.Base;
using Jankilla.Core.Converters;
using Jankilla.Core.Tags.Base;
using Jankilla.Core.UI.Forms;
using Jankilla.Core.UI.Forms.Base;
using Jankilla.Core.UI.Utils;
using Jankilla.Core.Utils;
using Jankilla.Driver.MitsubishiMxComponent;
using Jankilla.TagBuilder.Controls;
using Jankilla.TagBuilder.Controls.MitsubishiMxComponent;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Jankilla.TagBuilder
{
    public partial class MainForm : DevExpress.XtraEditors.XtraForm
    {
        #region Fields

        private DXMenuItem[] _driverMenuItems = new DXMenuItem[4];
        private DXMenuItem[] _deviceMenuItems = new DXMenuItem[4];
        private DXMenuItem[] _blockMenuItems = new DXMenuItem[3];

        private DXMenuItem _tagAddMenuItem;
        private DXMenuItem _tagEditMenuItem;
        private DXMenuItem _tagDeleteMenuItem;

        private string _titleWithVersion;

        private Project _loadedProject = new Project();



        #endregion

        #region Constructor
        public MainForm()
        {
            InitializeComponent();
        }

        #endregion

        #region Private Helpers

        private void newFile()
        {
            Text = _titleWithVersion;
            treeListChannels.ClearNodes();
            tagBindingSource.Clear();

            _loadedProject = new Project();
        }

        private void openFile(string path)
        {
            IOverlaySplashScreenHandle handle = DialogHelper.ShowProgressPanel(this);
            treeListChannels.ClearNodes();
            tagBindingSource.Clear();

            _loadedProject = JsonProjectHelper.Instance.OpenProjectFile(path);
            if (_loadedProject == null)
            {
                DialogHelper.CloseProgressPanel(handle);
                return;
            }

            foreach (var driver in _loadedProject.Drivers)
            {
                var root = treeListChannels.AppendNode(new object[] { driver.ToString() }, null, driver);
                foreach (var device in driver.Devices)
                {
                    var deviceNode = treeListChannels.AppendNode(new object[] { device.ToString() }, root, device);

                    foreach (var block in device.Blocks)
                    {
                        treeListChannels.AppendNode(new object[] { block.ToString() }, deviceNode, block);
                    }
                }
            }

            treeListChannels.ExpandAll();

            Text = $"{_titleWithVersion} [{path}]";

            DialogHelper.CloseProgressPanel(handle);
        }

        private void saveFile(string path)
        {
            JsonProjectHelper.Instance.SaveProjectFile(path, _loadedProject);

        }

        private void selectNode()
        {
            var focusedNode = treeListChannels.FocusedNode;
            if (focusedNode == null)
            {
                return;
            }

            tagBindingSource.Clear();
            switch (focusedNode.Level)
            {
                case 0:
                case 1:
                    layoutControlItemTags.Text = "Tags";
                    _tagAddMenuItem.Enabled = false;
                    break;
                case 2:
                    var block = focusedNode.Tag as Core.Contracts.Block;
                    layoutControlItemTags.Text = $"Tags - {block.Name}";
                    _tagAddMenuItem.Enabled = true;

                    foreach (var t in block.Tags)
                    {
                        tagBindingSource.Add(t);
                    }
                    break;
                default:
                    break;
            }

            tagBindingSource.ResetBindings(false);

        }

        private void createSample()
        {
            var project = new Project();
            Core.Contracts.Driver mxDriver = new MitsubishiMxComponentDriver();
            project.AddDriver(mxDriver);
            mxDriver.Name = "DRV01";
            mxDriver.Path = "DRV01";
            mxDriver.Description = "My Driver";
            mxDriver.ID = Guid.NewGuid();

            var mxDevice = new MitsubishiMxComponentDevice() { ID = Guid.NewGuid() };
            mxDevice.Name = "DV01";

            mxDriver.AddDevice(mxDevice);

            var myBlock = new MitsubishiMxComponentBlock { ID = Guid.NewGuid(), Name = "BLOCK 01", StationNo = 1, StartAddress = "D0000", BufferSize = 2000 };
            mxDevice.AddBlock(myBlock);
            mxDevice.AddBlock(new MitsubishiMxComponentBlock { ID = Guid.NewGuid(), Name = "BLOCK 02", StationNo = 1, StartAddress = "D1000", BufferSize = 2000 });
            mxDevice.AddBlock(new MitsubishiMxComponentBlock { ID = Guid.NewGuid(), Name = "BLOCK 03", StationNo = 1, StartAddress = "D2000", BufferSize = 2000 });

            var bitBlock = new MitsubishiMxComponentBlock { ID = Guid.NewGuid(), Name = "BLOCK 04", StationNo = 1, StartAddress = "M1000", BufferSize = 10 };
            mxDevice.AddBlock(bitBlock);

            int noCount = 0;

            myBlock.AddTag(new StringTag() { Name = "SAMPLE_STR_DATA_001", Address = "D0000", Direction = EDirection.In, ByteSize = 10, No = ++noCount, Category = "CDAT01", ID = Guid.NewGuid() });
            myBlock.AddTag(new StringTag() { Name = "SAMPLE_STR_DATA_002", Address = "D0010", Direction = EDirection.In, ByteSize = 10, No = ++noCount, Category = "CDAT01", ID = Guid.NewGuid() });
            myBlock.AddTag(new StringTag() { Name = "SAMPLE_STR_DATA_003", Address = "D0020", Direction = EDirection.In, ByteSize = 10, No = ++noCount, Category = "CDAT01", ID = Guid.NewGuid() });
            myBlock.AddTag(new StringTag() { Name = "SAMPLE_STR_DATA_004", Address = "D0030", Direction = EDirection.In, ByteSize = 10, No = ++noCount, Category = "CDAT01", ID = Guid.NewGuid() });

            myBlock.AddTag(new ShortTag() { Name = "SAMPLE_SRT_DATA_005", Address = "D0100", Direction = EDirection.In,No = ++noCount, Category = "CDAT01", ID = Guid.NewGuid() });
            myBlock.AddTag(new ShortTag() { Name = "SAMPLE_SRT_DATA_006", Address = "D0101", Direction = EDirection.In,No = ++noCount, Category = "CDAT01", ID = Guid.NewGuid() });
            myBlock.AddTag(new ShortTag() { Name = "SAMPLE_SRT_DATA_007", Address = "D0102", Direction = EDirection.In,No = ++noCount, Category = "CDAT01", ID = Guid.NewGuid() });
            myBlock.AddTag(new ShortTag() { Name = "SAMPLE_SRT_DATA_008", Address = "D0103", Direction = EDirection.In, No = ++noCount, Category = "CDAT01", ID = Guid.NewGuid() });

            myBlock.AddTag(new IntTag() { Name = "SAMPLE_INT_DATA_009", Address = "D0110", Direction = EDirection.In, No = ++noCount, Category = "CDAT01", ID = Guid.NewGuid() });
            myBlock.AddTag(new IntTag() { Name = "SAMPLE_INT_DATA_010", Address = "D0112", Direction = EDirection.In, No = ++noCount, Category = "CDAT01", ID = Guid.NewGuid() });
            myBlock.AddTag(new IntTag() { Name = "SAMPLE_INT_DATA_011", Address = "D0114", Direction = EDirection.In, No = ++noCount, Category = "CDAT01", ID = Guid.NewGuid() });
            myBlock.AddTag(new IntTag() { Name = "SAMPLE_INT_DATA_012", Address = "D0116", Direction = EDirection.In, No = ++noCount, Category = "CDAT01", ID = Guid.NewGuid() });

            bitBlock.AddTag(new BooleanTag() { Name = "SAMPLE_BOOL_DATA_013", Address = "M0000", Direction = EDirection.In, BitIndex = 0, No = ++noCount, Category = "CDAT01", ID = Guid.NewGuid() });
            bitBlock.AddTag(new BooleanTag() { Name = "SAMPLE_BOOL_DATA_014", Address = "M0001", Direction = EDirection.In, BitIndex = 1, No = ++noCount, Category = "CDAT01", ID = Guid.NewGuid() });
            bitBlock.AddTag(new BooleanTag() { Name = "SAMPLE_BOOL_DATA_015", Address = "M0002", Direction = EDirection.In, BitIndex = 2, No = ++noCount, Category = "CDAT01", ID = Guid.NewGuid() });
            bitBlock.AddTag(new BooleanTag() { Name = "SAMPLE_BOOL_DATA_016", Address = "M0003", Direction = EDirection.In, BitIndex = 3, No = ++noCount, Category = "CDAT01", ID = Guid.NewGuid() });

            string path = DialogHelper.ShowSaveFileDialog(DialogHelper.FILTER_STR_JSON);
            if (string.IsNullOrEmpty(path))
            {
                return;
            }

            _loadedProject = project;

            saveFile(path);

            openFile(path);
        }

        #endregion

        #region Events

        #region 공통

        private void mainForm_Load(object sender, EventArgs e)
        {

            #region Initialize Title

            _titleWithVersion = $"{Text} - V{Assembly.GetExecutingAssembly().GetName().Version}";
            Text = _titleWithVersion;

            #endregion

            #region Initialize Menu Items

            var imgAdd = svgImageCollection1[0];
            var imgRemove = svgImageCollection1[1];
            var imgEdit = svgImageCollection1[2];
            var imgCopy = svgImageCollection1[3];

            _driverMenuItems[0] = new DXMenuItem("Add Driver", addDriverMenuItem_Click, imgAdd, DXMenuItemPriority.Normal);
            _driverMenuItems[0].BeginGroup = true;

            _driverMenuItems[1] = new DXMenuItem("Edit Driver", editDriverMenuItem_Click, imgEdit, DXMenuItemPriority.Normal);
            _driverMenuItems[2] = new DXMenuItem("Delete Driver", deleteDriverMenuItem_Click, imgRemove, DXMenuItemPriority.Normal);

            _driverMenuItems[3] = new DXMenuItem("Add Device", addDeviceMenuItem_Click, imgAdd, DXMenuItemPriority.Normal);
            _driverMenuItems[3].BeginGroup = true;

            _deviceMenuItems[0] = new DXMenuItem("Edit Device", editDeviceMenuItem_Click, imgEdit, DXMenuItemPriority.Normal);
            _deviceMenuItems[0].BeginGroup = true;
            _deviceMenuItems[1] = new DXMenuItem("Delete Device", deleteDeviceMenuItem_Click, imgRemove, DXMenuItemPriority.Normal);
            _deviceMenuItems[2] = new DXMenuItem("Clone Device", null, imgCopy, DXMenuItemPriority.Normal);
            _deviceMenuItems[2].Enabled = false;
            _deviceMenuItems[3] = new DXMenuItem("Add Block", addBlockMenuItem_Click, imgAdd, DXMenuItemPriority.Normal);
            _deviceMenuItems[3].BeginGroup = true;

            _blockMenuItems[0] = new DXMenuItem("Edit Block", editBlockMenuItem_Click, imgEdit, DXMenuItemPriority.Normal);
            _blockMenuItems[0].BeginGroup = true;
            _blockMenuItems[1] = new DXMenuItem("Delete Block", deleteBlockMenuItem_Click, imgRemove, DXMenuItemPriority.Normal);
            _blockMenuItems[2] = new DXMenuItem("Clone Block", null, imgCopy, DXMenuItemPriority.Normal);
            _blockMenuItems[2].Enabled = false;

            _tagAddMenuItem = new DXMenuItem("Add Tag", addTagMenuItem_Click, imgAdd, DXMenuItemPriority.Normal);
            _tagAddMenuItem.BeginGroup = true;
            _tagEditMenuItem = new DXMenuItem("Edit Tag", editTagMenuItem_Click, imgEdit, DXMenuItemPriority.Normal);
            _tagDeleteMenuItem = new DXMenuItem("Delete Tag", deleteTagMenuItem_Click, imgRemove, DXMenuItemPriority.Normal);

            #endregion

            #region Initialize JSON Settings



            #endregion
        }

        #endregion

        #region 메뉴

        private void barButtonItemFileNew_ItemClick(object sender, ItemClickEventArgs e)
        {
            newFile();
        }

        private void barButtonItemFileOpen_ItemClick(object sender, ItemClickEventArgs e)
        {
            var path = DialogHelper.ShowOpenFileDialog(DialogHelper.FILTER_STR_JSON);
            if (string.IsNullOrEmpty(path))
            {
                return;
            }

            openFile(path);
        }

        private void barButtonItemFileSave_ItemClick(object sender, ItemClickEventArgs e)
        {
            string path = DialogHelper.ShowSaveFileDialog(DialogHelper.FILTER_STR_JSON);
            if (string.IsNullOrEmpty(path))
            {
                return;
            }

            saveFile(path);
        }

        private void barButtonItemCreateSample_ItemClick(object sender, ItemClickEventArgs e)
        {
            createSample();
        }

        #endregion

        #region 좌측 트리

        private void treeListChannels_FocusedNodeChanged(object sender, DevExpress.XtraTreeList.FocusedNodeChangedEventArgs e)
        {
            selectNode();
        }

        private void treeListChannels_PopupMenuShowing(object sender, DevExpress.XtraTreeList.PopupMenuShowingEventArgs e)
        {
            if (e.Menu?.Items == null)
            {
                return;
            }

            if (e.HitInfo.HitInfoType == DevExpress.XtraTreeList.HitInfoType.Empty)
            {
                e.Menu.Items.Add(_driverMenuItems[0]);
                return;
            }

            if (!e.HitInfo.InRow) return;

            var node = e.HitInfo.Node;
            if (node == null) return;

            if (node.Level == 0)
            {
                foreach (var item in _driverMenuItems)
                {
                    e.Menu.Items.Add(item);
                }
            }
            else if (node.Level == 1)
            {
                var device = node.Tag as Device;
                if (device == null) return;

                foreach (var item in _deviceMenuItems)
                {
                    item.Tag = node;
                    e.Menu.Items.Add(item);
                }
            }
            else if (node.Level == 2)
            {
                var command = node.Tag as Block;
                if (command == null) return;

                foreach (var item in _blockMenuItems)
                {
                    item.Tag = node;
                    e.Menu.Items.Add(item);
                }
            }
        }

        private void addDriverMenuItem_Click(object sender, EventArgs e)
        {
            var obj = DialogHelper.ShowInputComboMessageBox<EDriverDiscriminator>("Add Driver", "select the type of driver you want to add");
            if (obj == null)
            {
                return;
            }

            var driverType = (EDriverDiscriminator)obj;

            IDataAccessControl control = null;

            switch (driverType)
            {
                case EDriverDiscriminator.MitsubishiMxComponent:
                    control = new MitsubishiMxComponentDriverUserControl()
                    {
                        Project = _loadedProject,
                        Command = EControlCommand.Create
                    };
                    break;
                default:
                    Debug.Assert(false);
                    break;
            }

            using (var createForm = new DataAccessForm())
            {
                createForm.IconOptions.Icon = this.IconOptions.Icon;
                createForm.Text = "Add Driver";
                createForm.MainView = control;
                if (createForm.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                var driver = (Jankilla.Core.Contracts.Driver)createForm.Result;
                if (driver == null)
                {
                    return;
                }

                treeListChannels.AppendNode(new object[] { driver.ToString() }, null, driver);
            }
        }

        private void editDriverMenuItem_Click(object sender, EventArgs e)
        {
            var driver = treeListChannels.FocusedNode?.Tag as Core.Contracts.Driver;
            if (driver == null)
            {
                return;
            }

            IDataAccessControl control = null;
            switch (driver.Discriminator)
            {
                case "MitsubishiMxComponent":
                    var mitsubishiMxComponentDriverUserControl = new MitsubishiMxComponentDriverUserControl()
                    {
                        Project = _loadedProject,
                        Command = EControlCommand.Edit
                    };
                    control = mitsubishiMxComponentDriverUserControl;
                    IBindableControl<MitsubishiMxComponentDriver> bindableControl = mitsubishiMxComponentDriverUserControl;
                    bindableControl.Bind((MitsubishiMxComponentDriver)driver);
                    break;
                default:
                    Debug.Assert(false);
                    break;
            }

            using (var createForm = new DataAccessForm())
            {
                createForm.IconOptions.Icon = this.IconOptions.Icon;
                createForm.Text = "Edit Driver";
                createForm.MainView = control;

                if (createForm.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                var editedDriver = (Jankilla.Core.Contracts.Driver)createForm.Result;
                if (editedDriver == null)
                {
                    return;
                }

                var node = treeListChannels.FindNodeByID(treeListChannels.FocusedNode.Id);
                node[0] = editedDriver.ToString();
                node.Tag = editedDriver;
            }
        }

        private void deleteDriverMenuItem_Click(object sender, EventArgs e)
        {
            Debug.Assert(_loadedProject?.Drivers != null);
            var driver = treeListChannels.FocusedNode?.Tag as Jankilla.Core.Contracts.Driver;
            if (driver == null)
            {
                return;
            }

            var result = DialogHelper.ShowMessageBoxDialog("Are you sure you want to delete it?");
            if (result != DialogResult.OK)
            {
                return;
            }

            _loadedProject.RemoveDriver(driver);
            treeListChannels.DeleteNode(treeListChannels.FocusedNode);
        }

        private void addDeviceMenuItem_Click(object sender, EventArgs e)
        {
            var driver = treeListChannels.FocusedNode?.Tag as Core.Contracts.Driver;
            if (driver == null)
            {
                return;
            }

            IDataAccessControl control = null;

            switch (driver.Discriminator)
            {
                case "MitsubishiMxComponent":
                    control = new MitsubishiMxComponentDeviceUserControl()
                    {
                        Driver = (MitsubishiMxComponentDriver)driver,
                        Command = EControlCommand.Create
                    };
                    break;
                default:
                    Debug.Assert(false);
                    break;
            }

            using (var createForm = new DataAccessForm())
            {
                createForm.IconOptions.Icon = this.IconOptions.Icon;
                createForm.Text = "Add Device";
                createForm.MainView = control;
                if (createForm.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                var device = (Jankilla.Core.Contracts.Device)createForm.Result;
                if (device == null)
                {
                    return;
                }

                treeListChannels.AppendNode(new object[] { device }, treeListChannels.FocusedNode, device);
            }

        }

        private void editDeviceMenuItem_Click(object sender, EventArgs e)
        {
            var device = treeListChannels.FocusedNode?.Tag as Core.Contracts.Device;
            if (device == null)
            {
                return;
            }
            var driver = treeListChannels.FocusedNode?.ParentNode?.Tag as Core.Contracts.Driver;
            if (driver == null)
            {
                return;
            }

            IDataAccessControl control = null;

            switch (device.Discriminator)
            {
                case "MitsubishiMxComponent":
                    var mitsubishiMxComponentDeviceUserControl = new MitsubishiMxComponentDeviceUserControl()
                    {
                        Driver = (MitsubishiMxComponentDriver)driver,
                        Command = EControlCommand.Edit
                    };
                    control = mitsubishiMxComponentDeviceUserControl;
                    IBindableControl<MitsubishiMxComponentDevice> bindableControl = mitsubishiMxComponentDeviceUserControl;
                    bindableControl.Bind((MitsubishiMxComponentDevice)device);
                    break;
                default:
                    Debug.Assert(false);
                    break;
            }

            using (var createForm = new DataAccessForm())
            {
                createForm.IconOptions.Icon = this.IconOptions.Icon;
                createForm.Text = "Edit Device";
                createForm.MainView = control;
                if (createForm.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                var editedDevice = (Jankilla.Core.Contracts.Device)createForm.Result;
                if (editedDevice == null)
                {
                    return;
                }

                var node = treeListChannels.FindNodeByID(treeListChannels.FocusedNode.Id);
                node[0] = editedDevice.ToString();
                node.Tag = editedDevice;

            }
        }

        private void deleteDeviceMenuItem_Click(object sender, EventArgs e)
        {
            Debug.Assert(_loadedProject?.Drivers != null);
            var driver = treeListChannels.FocusedNode?.ParentNode?.Tag as Jankilla.Core.Contracts.Driver;
            if (driver == null)
            {
                return;
            }

            var device = treeListChannels.FocusedNode?.Tag as Jankilla.Core.Contracts.Device;
            if (device == null)
            {
                return;
            }

            var result = DialogHelper.ShowMessageBoxDialog("Are you sure you want to delete it?");
            if (result != DialogResult.OK)
            {
                return;
            }

            driver.RemoveDevice(device);
            treeListChannels.DeleteNode(treeListChannels.FocusedNode);
        }

        private void addBlockMenuItem_Click(object sender, EventArgs e)
        {
            var device = treeListChannels.FocusedNode?.Tag as Core.Contracts.Device;
            if (device == null)
            {
                return;
            }

            IDataAccessControl control = null;

            switch (device.Discriminator)
            {
                case "MitsubishiMxComponent":
                    control = new MitsubishiMxComponentBlockUserControl()
                    {
                        Device = (MitsubishiMxComponentDevice)device,
                        Command = EControlCommand.Create
                    };
                    break;
                default:
                    Debug.Assert(false);
                    break;
            }

            using (var createForm = new DataAccessForm())
            {
                createForm.IconOptions.Icon = this.IconOptions.Icon;
                createForm.Text = "Add Block";
                createForm.MainView = control;
                if (createForm.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                var block = (Jankilla.Core.Contracts.Block)createForm.Result;
                if (block == null)
                {
                    return;
                }

                treeListChannels.AppendNode(new object[] { block }, treeListChannels.FocusedNode, block);
            }

        }

        private void editBlockMenuItem_Click(object sender, EventArgs e)
        {
            var block = treeListChannels.FocusedNode?.Tag as Core.Contracts.Block;
            if (block == null)
            {
                return;
            }
            var device = treeListChannels.FocusedNode?.ParentNode?.Tag as Core.Contracts.Device;
            if (device == null)
            {
                return;
            }

            IDataAccessControl control = null;

            switch (block.Discriminator)
            {
                case "MitsubishiMxComponent":
                    var mitsubishiMxComponentBlockUserControl = new MitsubishiMxComponentBlockUserControl()
                    {
                        Device = (MitsubishiMxComponentDevice)device,
                        Command = EControlCommand.Edit
                    };
                    control = mitsubishiMxComponentBlockUserControl;
                    IBindableControl<MitsubishiMxComponentBlock> bindableControl = mitsubishiMxComponentBlockUserControl;
                    bindableControl.Bind((MitsubishiMxComponentBlock)block);
                    break;
                default:
                    Debug.Assert(false);
                    break;
            }

            using (var createForm = new DataAccessForm())
            {
                createForm.IconOptions.Icon = this.IconOptions.Icon;
                createForm.Text = "Edit Block";
                createForm.MainView = control;
                if (createForm.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                var editedblock = (Jankilla.Core.Contracts.Block)createForm.Result;
                if (editedblock == null)
                {
                    return;
                }

                var node = treeListChannels.FindNodeByID(treeListChannels.FocusedNode.Id);
                node[0] = editedblock.ToString();
                node.Tag = null;
                node.Tag = editedblock;

            }
        }

        private void deleteBlockMenuItem_Click(object sender, EventArgs e)
        {
            Debug.Assert(_loadedProject?.Drivers != null);
            var device = treeListChannels.FocusedNode?.ParentNode?.Tag as Jankilla.Core.Contracts.Device;
            if (device == null)
            {
                return;
            }

            var block = treeListChannels.FocusedNode?.Tag as Jankilla.Core.Contracts.Block;
            if (block == null)
            {
                return;
            }

            var result = DialogHelper.ShowMessageBoxDialog("Are you sure you want to delete it?");
            if (result != DialogResult.OK)
            {
                return;
            }

            device.RemoveBlock(block);
            treeListChannels.DeleteNode(treeListChannels.FocusedNode);
        }

        #endregion

        #region 태그 그리드

        private void addTagMenuItem_Click(object sender, EventArgs e)
        {
            var block = treeListChannels.FocusedNode.Tag as Core.Contracts.Block;
            if (block == null)
            {
                return;
            }

            IDataAccessControl control = null;

            switch (block.Discriminator)
            {
                case "MitsubishiMxComponent":
                    control = new MitsubishiMxComponentTagUserControl()
                    {
                        Block = (MitsubishiMxComponentBlock)block,
                        Command = EControlCommand.Create,
                        No = $"{block.Tags.Count() + 1}",
                        ID = Guid.NewGuid().ToString()
                    };
                    break;
                default:
                    Debug.Assert(false);
                    break;
            }

            using (var createForm = new DataAccessForm())
            {
                createForm.IconOptions.Icon = this.IconOptions.Icon;
                createForm.Text = "Add Tag";
                createForm.MainView = control;

                if (createForm.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                var tag = (Tag)createForm.Result;
                if (tag == null)
                {
                    return;
                }

                tagBindingSource.Add(tag);
            }
        }

        private void editTagMenuItem_Click(object sender, EventArgs e)
        {
            var tag = gridViewTags.GetFocusedRow() as Core.Contracts.Tags.Tag;
            if (tag == null)
            {
                return;
            }
            var block = treeListChannels.FocusedNode?.Tag as Core.Contracts.Block;
            if (block == null)
            {
                return;
            }

            IDataAccessControl control = null;

            switch (block.Discriminator)
            {
                case "MitsubishiMxComponent":
                    var mitsubishiMxComponentTagUserControl = new MitsubishiMxComponentTagUserControl()
                    {
                        Block = (MitsubishiMxComponentBlock)block,
                        Command = EControlCommand.Edit
                    };
                    control = mitsubishiMxComponentTagUserControl;
                    IBindableControl<Tag> bindableControl = mitsubishiMxComponentTagUserControl;
                    bindableControl.Bind(tag);
                    break;
                default:
                    Debug.Assert(false);
                    break;
            }

            using (var createForm = new DataAccessForm())
            {
                createForm.IconOptions.Icon = this.IconOptions.Icon;
                createForm.Text = "Edit Block";
                createForm.MainView = control;
                if (createForm.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                var editedTag = (Jankilla.Core.Contracts.Tags.Tag)createForm.Result;
                if (editedTag == null)
                {
                    return;
                }

                int index = tagBindingSource.IndexOf(tag);
                if (index < 0)
                {
                    return;
                }
                tagBindingSource[index] = editedTag;
            }
        }

        private void deleteTagMenuItem_Click(object sender, EventArgs e)
        {
            Debug.Assert(_loadedProject?.Drivers != null);

            var block = treeListChannels.FocusedNode?.Tag as Jankilla.Core.Contracts.Block;
            if (block == null)
            {
                return;
            }

            var tag = gridViewTags.GetFocusedRow() as Tag;
            if (tag == null)
            {
                return;
            }

            var result = DialogHelper.ShowMessageBoxDialog("Are you sure you want to delete it?");
            if (result != DialogResult.OK)
            {
                return;
            }

            block.RemoveTag(tag);
            tagBindingSource.Remove(tag);

        }

        private void gridViewTags_PopupMenuShowing(object sender, DevExpress.XtraGrid.Views.Grid.PopupMenuShowingEventArgs e)
        {
            var focusedNode = treeListChannels.FocusedNode;
            if (focusedNode == null)
            {
                return;
            }

            if (e.Menu?.Items == null)
            {
                e.Menu = new DevExpress.XtraGrid.Menu.GridViewMenu(gridViewTags);
            }

            if (e.HitInfo.InRow)
            {
                // Add / Modify / Insert
                e.Menu.Items.Add(_tagAddMenuItem);
                e.Menu.Items.Add(_tagEditMenuItem);
                e.Menu.Items.Add(_tagDeleteMenuItem);
                return;
            }

            if (e.HitInfo.HitTest == DevExpress.XtraGrid.Views.Grid.ViewInfo.GridHitTest.EmptyRow)
            {
                // Add 
                e.Menu.Items.Add(_tagAddMenuItem);
                return;
            }


        }



        #endregion

        #region 라이센스

        private void barStaticItemLicense_ItemDoubleClick(object sender, ItemClickEventArgs e)
        {
#if DEBUG
            using (var licenseForm = new LicenseAdminForm())
            {
                licenseForm.IconOptions.Icon = this.IconOptions.Icon;
                licenseForm.ShowDialog();
            }
#endif
        }

        #endregion

        #endregion


    }



}