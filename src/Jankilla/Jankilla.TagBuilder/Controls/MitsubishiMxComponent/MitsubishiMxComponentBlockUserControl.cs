using DevExpress.XtraEditors;
using Jankilla.Core.UI.WinForms.Forms.Base;
using Jankilla.Driver.MitsubishiMxComponent;
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

namespace Jankilla.TagBuilder.Controls.MitsubishiMxComponent
{
    public partial class MitsubishiMxComponentBlockUserControl : XtraUserControl, IDataAccessControl, IBindableControl<MitsubishiMxComponentBlock>
    {
        public MitsubishiMxComponentDevice Device { get; set; }
        private MitsubishiMxComponentBlock _result;

        public MitsubishiMxComponentBlockUserControl()
        {
            InitializeComponent();
        }

        public Control Control => this;

        public EControlCommand Command { get; set; }

        public void Bind(MitsubishiMxComponentBlock obj)
        {
            textEditBlockName.Text = obj.Name;
            textEditDescription.Text = obj.Description;
            spinEditWordSize.Value = obj.BufferSize / 2;
            spinEditStationNo.Value = obj.StationNo;

            if (string.IsNullOrEmpty(obj.StartAddress))
            {
                return;
            }
            
            if (obj.StartAddress.Length < 2)
            {
                return;
            }

            comboBoxEditDeviceType.Properties.Items.Clear();
            HashSet<string> sets = null;
            if (obj.DeviceType == EDeviceType.Bit)
            {
                checkEditIsBitArea.Checked = true;
                sets = MitsubishiMxComponentDriver.BitDeviceTypes;
            }
            else
            {
                checkEditIsBitArea.Checked = false;
                sets = MitsubishiMxComponentDriver.WordDeviceTypes;
            }

            var oSets = sets.OrderByDescending(o => o.Length);
            int cnt = 0;
            var dType = obj.DeviceCode;
            foreach (var item in oSets)
            {
                comboBoxEditDeviceType.Properties.Items.Add(item);
                if (item == dType)
                {
                    comboBoxEditDeviceType.SelectedIndex = cnt;
                }
                ++cnt;
            }

            var address = obj.StartAddress.Substring(1);

            textEditAddress.Text = address;

            _result = obj;
        }

        public object Do(ref string errorMessage)
        {
            if (Device == null)
            {
                errorMessage = "The device is not set to the current control.";
                return null;
            }

            if (string.IsNullOrEmpty(textEditBlockName.Text))
            {
                errorMessage = "The block name cannot be a space.";
                return null;
            }


            switch (Command)
            {
                case EControlCommand.Create:
                    return create(ref errorMessage);
                case EControlCommand.Edit:
                    return edit(ref errorMessage);
                default:
                    Debug.Assert(false);
                    return null;
            }
        }

        private object create(ref string errorMessage)
        {
            if (Device.Blocks.Any(d => d.Name == textEditBlockName.Text))
            {
                errorMessage = "Duplicate block names are not allowed.";
                return null;
            }

            string name = textEditBlockName.Text;
            string desc = textEditDescription.Text;
            int sNo = (int)spinEditStationNo.Value;
            int byteSize = ((int)spinEditWordSize.Value) * 2;

            var dType = comboBoxEditDeviceType.SelectedItem.ToString();
            var address = textEditAddress.Text;

            var block = new MitsubishiMxComponentBlock{ ID = Guid.NewGuid(), Name = name, StationNo = sNo, StartAddress = $"{dType}{address}", BufferSize = byteSize };
            block.Description = desc;

            _result = block;

            bool bAdded = Device.AddBlock(_result);
            if (!bAdded)
            {
                errorMessage = "Unable to add block due to internal issues.";
                return null;
            }

            return _result;
        }

        private object edit(ref string errorMessage)
        {
            var blocks = Device.Blocks.ToList();
            blocks.Remove(_result);

            if (blocks.Any(d => d.Name == textEditBlockName.Text))
            {
                errorMessage = "Duplicate block names are not allowed.";
                return null;
            }

            int index = Array.IndexOf(Device.Blocks.ToArray(), _result);
            if (index < 0)
            {
                errorMessage = "The block doesn't exist.";
                return null;
            }

            string name = textEditBlockName.Text;
            string desc = textEditDescription.Text;
            int sNo = (int)spinEditStationNo.Value;
            int byteSize = ((int)spinEditWordSize.Value) * 2;

            var dType = comboBoxEditDeviceType.SelectedItem.ToString();
            var address = textEditAddress.Text;

            var block = new MitsubishiMxComponentBlock{ ID = _result.ID, Name = name, StationNo = sNo, StartAddress = $"{dType}{address}", BufferSize = byteSize };
            block.Description = desc;

            Device.ReplaceBlock(index, block);

            foreach (var tag in _result.Tags)
            {
                block.AddTag(tag);
            }
            _result.RemoveAllTags();

            _result = block;

            return _result;
        }

        private void mitsubishiMxComponentBlockUserControl_Load(object sender, EventArgs e)
        {
            if (_result == null)
            {
                foreach (var item in MitsubishiMxComponentDriver.WordDeviceTypes)
                {
                    comboBoxEditDeviceType.Properties.Items.Add(item);
                }

                comboBoxEditDeviceType.SelectedItem = "D";
            }
        }

        private void checkEditIsBitArea_CheckedChanged(object sender, EventArgs e)
        {
            comboBoxEditDeviceType.Properties.Items.Clear();

            if (checkEditIsBitArea.Checked)
            {
                foreach (var item in MitsubishiMxComponentDriver.BitDeviceTypes)
                {
                    comboBoxEditDeviceType.Properties.Items.Add(item);
                }

                comboBoxEditDeviceType.SelectedItem = "M";

                return;
            }

            foreach (var item in MitsubishiMxComponentDriver.WordDeviceTypes)
            {
                comboBoxEditDeviceType.Properties.Items.Add(item);
            }

            comboBoxEditDeviceType.SelectedItem = "D";
        }
    }
}
