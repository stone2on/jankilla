using DevExpress.XtraEditors;
using Jankilla.Core.Contracts;
using Jankilla.Core.UI.WinForms.Forms.Base;
using Jankilla.Core.Utils;
using Jankilla.Driver.MitsubishiMxComponent;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Jankilla.TagBuilder.Controls.MitsubishiMxComponent
{
    public partial class MitsubishiMxComponentDriverUserControl : XtraUserControl, IDataAccessControl, IBindableControl<MitsubishiMxComponentDriver>
    {
        public Project Project { get; set; }

        private MitsubishiMxComponentDriver _result;

        public MitsubishiMxComponentDriverUserControl()
        {
            InitializeComponent();
        }

        public Control Control => this;

        public EControlCommand Command { get; set; }

        public void Bind(MitsubishiMxComponentDriver obj)
        {
            textEditDriverName.Text = obj.Name;
            textEditDescription.Text = obj.Description;

            _result = obj;
        }

        public object Do(ref string errorMessage)
        {
            if (Project == null)
            {
                errorMessage = "The project is not set to the current control.";
                return null;
            }

            if (string.IsNullOrEmpty(textEditDriverName.Text))
            {
                errorMessage = "The driver name cannot be a space.";
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
                    return _result;
            }
        }

        private object create(ref string errorMessage)
        {
            if (Project.Drivers.Any(d => d.Name == textEditDriverName.Text))
            {
                errorMessage = "Duplicate driver names are not allowed.";
                return null;
            }

            _result = new MitsubishiMxComponentDriver()
            {
                ID = Guid.NewGuid(),
                Name = textEditDriverName.Text,
                Path = textEditDriverName.Text,
                Description = textEditDescription.Text
            };

            ValidationResult validationResult = Project.AddDriver(_result);
            if (!validationResult.IsValid)
            {
                errorMessage = $"Unable to add driver due to internal issues : {validationResult.Message}";
                return null;
            }

            return _result;
        }

        private object edit(ref string errorMessage)
        {
            if (Project.Drivers.Where(d => d != _result).Any(d => d.Name == textEditDriverName.Text))
            {
                errorMessage = "Duplicate driver names are not allowed.";
                return null;
            }

            _result.Name = textEditDriverName.Text;
            _result.Description = textEditDescription.Text;

            return _result;
        }

    }
}
