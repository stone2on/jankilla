using DevExpress.XtraEditors;
using Jankilla.Core.Contracts;
using Jankilla.Core.Contracts.Tags;
using Jankilla.Core.Contracts.Tags.Base;
using Jankilla.Core.Tags.Base;
using Jankilla.Core.UI.Forms.Base;
using Jankilla.Driver.MitsubishiMxComponent;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Jankilla.TagBuilder.Controls.MitsubishiMxComponent
{
    public partial class MitsubishiMxComponentTagUserControl : XtraUserControl, IDataAccessControl, IBindableControl<Tag>
    {
        public MitsubishiMxComponentBlock Block { get; set; }

        public ETagDiscriminator Discriminator { get; private set; }

        public string ID
        {
            get
            {
                return textEditID.Text;
            }
            set
            {
                textEditID.Text = value;
            }
        }

        public string No
        {
            get
            {
                return textEditNo.Text;
            }
            set
            {
                textEditNo.Text = value;
            }
        }

        private Tag _result;

        public MitsubishiMxComponentTagUserControl()
        {
            InitializeComponent();
        }

        public Control Control => this;

        public EControlCommand Command { get; set; }

        public void Bind(Tag obj)
        {
            textEditID.Text = obj.ID.ToString();
            textEditNo.Text = obj.No.ToString();
            textEditName.Text = obj.Name;
            textEditAddress.Text = obj.Address;
            textEditCategory.Text = obj.Category;
            textEditDescription.Text = obj.Description;
            textEditUnit.Text = obj.Unit;
            textEditPath.Text = obj.Path;
           

            spinEditByteSize.Value = obj.ByteSize;
            imageComboBoxEditDirection.SelectedIndex = (int)obj.Direction;
            imageComboBoxEditDiscriminator.SelectedIndex = (int)obj.Discriminator;


            textEditFactor.Value = (decimal)obj.Factor;
            textEditOffset.Value = (decimal)obj.Offset;
            checkEditUseFactor.Checked = obj.UseFactor;
            checkEditUseOffset.Checked = obj.UseOffset;


            if (obj.Discriminator == ETagDiscriminator.Boolean)
            {
                spinEditBitIndex.Value = ((BooleanTag)obj).BitIndex;
            }

            if (Command == EControlCommand.Edit)
            {
                textEditPath.ReadOnly = false;
            }

            _result = obj;
        }

        public object Do(ref string errorMessage)
        {
            //int id = int.Parse(textEditID.Text);
            int no = int.Parse(textEditNo.Text);
            string name = textEditName.Text;
            string address = textEditAddress.Text;
            string category = textEditCategory.Text;
            string desc = textEditDescription.Text;
            string unit = textEditUnit.Text;
            string path = textEditPath.Text;
            int byteSize = (int)spinEditByteSize.Value;

            if (Block == null)
            {
                errorMessage = "The block is not set to the current control";
                return null;
            }

            if (string.IsNullOrEmpty(name))
            {
                errorMessage = "The name cannot be a space.";
                return null;
            }

            if (string.IsNullOrEmpty(address))
            {
                errorMessage = "The address can't be a space.";
                return null;
            }

            if (!address.StartsWith(Block.DeviceCode))
            {
                errorMessage = "Block device code and address don't match";
                return null;
            }

            if (address.Length - Block.DeviceCode.Length < 1)
            {
                errorMessage = "Check the address format";
                return null;
            }

            string strNum = address.Substring(Block.DeviceCode.Length);

            //int num = Block.DeviceNumber != EDeviceNumber.Hex ? int.Parse(strNum) : int.Parse(strNum, NumberStyles.HexNumber);

            bool bParsed;
            int num;

            if (Block.DeviceNumber != EDeviceNumber.Hex)
            {
                bParsed = int.TryParse(strNum, out num);
            }
            else
            {
                bParsed = int.TryParse(strNum, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out num);
            }

            if (!bParsed)
            {
                errorMessage = "Check the address format";
                return null;
            }

            if (Block.DeviceType == EDeviceType.Word)
            {
                if (num < Block.StartAddressNo || num + (byteSize / 2) > Block.StartAddressNo + (Block.BufferSize / 2))
                {
                    errorMessage = "The tag address is out of scope for the block.";
                    return null;
                }
            }
            else if (num < Block.StartAddressNo || num > Block.StartAddressNo + ((Block.BufferSize / 2) * 16))
            {
                errorMessage = "The tag address is out of scope for the block.";
                return null;
            }

            var direction = imageComboBoxEditDirection.SelectedItem?.ToString();
            bParsed = Enum.TryParse(direction, out EDirection tagDirection);
            if (!bParsed)
            {
                errorMessage = "The type cannot be detected.";
                return null;
            }

            Tag tmp = null;

            switch (Discriminator)
            {
                case ETagDiscriminator.Boolean:
                    int bitIndex = (int)spinEditBitIndex.Value;
                    tmp = new BooleanTag(){ Name = name, Address = address, Direction = tagDirection, BitIndex = bitIndex };
                    break;
                case ETagDiscriminator.Int:
                    tmp = new IntTag() 
                    { 
                        Name = name, 
                        Address = address, 
                        Direction = tagDirection,
                        UseFactor = checkEditUseFactor.Checked,
                        Factor = (double)textEditFactor.Value,
                        UseOffset = checkEditUseOffset.Checked,
                        Offset = (double)textEditOffset.Value,
                    };
                    break;
                case ETagDiscriminator.Short:
                    tmp = new ShortTag()
                    {
                        Name = name,
                        Address = address,
                        Direction = tagDirection,
                        UseFactor = checkEditUseFactor.Checked,
                        Factor = (double)textEditFactor.Value,
                        UseOffset = checkEditUseOffset.Checked,
                        Offset = (double)textEditOffset.Value,
                    };
                    break;
                case ETagDiscriminator.String:
                    tmp = new StringTag()
                    {
                        Name = name,
                        Address = address,
                        Direction = tagDirection,
                        ByteSize = byteSize
                    };
                    break;
                case ETagDiscriminator.Float:
                    tmp = new FloatTag()
                    {
                        Name = name,
                        Address = address,
                        Direction = tagDirection,
                        UseFactor = checkEditUseFactor.Checked,
                        Factor = (double)textEditFactor.Value,
                        UseOffset = checkEditUseOffset.Checked,
                        Offset = (double)textEditOffset.Value,
                    };
                    break;
                default:
                    Debug.Assert(false);
                    break;
            }
            tmp.ID = Guid.NewGuid();
            tmp.No = no;
            tmp.Category = category;
            tmp.Description = desc;
            tmp.Unit = unit;
            tmp.Path = path;
            tmp.BlockID = Block.ID;

            switch (Command)
            {
                case EControlCommand.Create:
                    return create(ref errorMessage, tmp);
                case EControlCommand.Edit:
                    return edit(ref errorMessage, tmp);
                default:
                    Debug.Assert(false);
                    return null;
            }
        }

        private object create(ref string errorMessage, Tag tmp)
        {
            if (Block.Tags.Any(t => t.ID == tmp.ID))
            {
                errorMessage = "Duplicate tag ids are not allowed.";
                return null;
            }

            _result = tmp;

            bool bAdded = Block.AddTag(_result);
            if (!bAdded)
            {
                errorMessage = "Unable to add tag due to internal issues.";
                return null;
            }

            return _result;
        }

        private object edit(ref string errorMessage, Tag tmp)
        {
            Debug.Assert(_result != null);

            int index = Block.Tags.IndexOf(_result);
            if (index < 0)
            {
                errorMessage = "The tag doesn't exist.";
                return null;
            }

            Block.Tags[index] = tmp;

            _result = tmp;
            return _result;
        }


        private void imageComboBoxEditDiscriminator_SelectedIndexChanged(object sender, EventArgs e)
        {
            var discriminator = imageComboBoxEditDiscriminator.SelectedItem?.ToString();
            bool bParsed = Enum.TryParse(discriminator, out ETagDiscriminator tagDiscriminator);

            Discriminator = tagDiscriminator;

            if (!bParsed)
            {
                return;
            }

            switch (Discriminator)
            {
                case ETagDiscriminator.Boolean:
                    checkEditUseFactor.Checked = false;
                    checkEditUseOffset.Checked = false;
                    checkEditUseFactor.Enabled = false;
                    checkEditUseOffset.Enabled = false;
                    spinEditBitIndex.Enabled = true;
                    spinEditByteSize.Enabled = false;
                    spinEditByteSize.Value = 2;
                    break;
                case ETagDiscriminator.String:
                    checkEditUseFactor.Checked = false;
                    checkEditUseOffset.Checked = false;
                    checkEditUseFactor.Enabled = false;
                    checkEditUseOffset.Enabled = false;
                    spinEditBitIndex.Enabled = false;
                    spinEditByteSize.Enabled = true;
                    spinEditByteSize.Value = 20;
                    break;
                case ETagDiscriminator.Float:
                case ETagDiscriminator.Int:
                    spinEditByteSize.Value = 4;
                    spinEditByteSize.Enabled = false;
                    spinEditBitIndex.Enabled = false;
                    checkEditUseFactor.Enabled = true;
                    checkEditUseOffset.Enabled = true;
                    break;
                case ETagDiscriminator.Short:
                    spinEditByteSize.Value = 2;
                    spinEditByteSize.Enabled = false;
                    spinEditBitIndex.Enabled = false;
                    checkEditUseFactor.Enabled = true;
                    checkEditUseOffset.Enabled = true;
                    break;
                default:
                    Debug.Assert(false);
                    break;
            }
        }

        private void checkEditUseOffset_CheckedChanged(object sender, EventArgs e)
        {
            textEditOffset.Enabled = checkEditUseOffset.Checked;
        }

        private void checkEditUseFactor_CheckedChanged(object sender, EventArgs e)
        {
            textEditFactor.Enabled = checkEditUseFactor.Checked;
        }

   
    }
}
