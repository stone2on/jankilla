using DevExpress.XtraEditors;
using Jankilla.Core.UI.Forms.Base;
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
    public partial class MitsubishiMxComponentDeviceUserControl : DevExpress.XtraEditors.XtraUserControl, IDataAccessControl, IBindableControl<MitsubishiMxComponentDevice>
    {
        public MitsubishiMxComponentDriver Driver { get; set; }
        private MitsubishiMxComponentDevice _result;

        public MitsubishiMxComponentDeviceUserControl()
        {
            InitializeComponent();
        }

        public Control Control => this;

        public EControlCommand Command { get; set; }

        public void Bind(MitsubishiMxComponentDevice obj)
        {
            textEditDeviceName.Text = obj.Name;
            textEditDescription.Text = obj.Description;

            _result = obj;
        }

        public object Do(ref string errorMessage)
        {
            if (Driver == null)
            {
                errorMessage = "The driver is not set to the current control.";
                return null;
            }

            if (string.IsNullOrEmpty(textEditDeviceName.Text))
            {
                errorMessage = "The device name cannot be a space.";
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
            if (Driver.Devices.Any(d => d.Name == textEditDeviceName.Text))
            {
                errorMessage = "Duplicate device names are not allowed.";
                return null;
            }

            _result = new MitsubishiMxComponentDevice()
            {
                ID = Guid.NewGuid(),
                Name = textEditDeviceName.Text,
                Description = textEditDescription.Text
            };

            bool bAdded = Driver.AddDevice(_result);
            if (!bAdded)
            {
                errorMessage = "Unable to add device due to internal issues.";
                return null;
            }

            return _result;
        }

        private object edit(ref string errorMessage)
        {
            if (Driver.Devices.Where(d => d != _result).Any(d => d.Name == textEditDeviceName.Text))
            {
                errorMessage = "Duplicate device names are not allowed.";
                return null;
            }

            _result.Name = textEditDeviceName.Text;
            _result.Description = textEditDescription.Text;

            return _result;
        }

    }
}
