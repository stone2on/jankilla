using DevExpress.XtraEditors;
using Jankilla.Core.UI.WinForms.Forms.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Jankilla.Core.UI.WinForms.Forms
{
    public partial class DataAccessForm : DevExpress.XtraEditors.XtraForm
    {
        private IDataAccessControl _dataAccess;
        public IDataAccessControl MainView 
        {
            get { return _dataAccess; }
            set
            {
                _dataAccess = value;
                panelControlMain.Controls.Clear();

                if (value == null)
                {
                    return;
                }

                panelControlMain.Controls.Add(_dataAccess.Control);
                _dataAccess.Control.Dock = DockStyle.Fill;
            }
        }

        public object Result { get; private set; }

        public DataAccessForm()
        {
            InitializeComponent();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            string errorMessage = null;
            Result = _dataAccess.Do(ref errorMessage);

            if (Result == null)
            {
                simpleLabelItemErrorMsg.ImageOptions.SvgImage = svgImageCollection1[1];
                simpleLabelItemErrorMsg.AppearanceItemCaption.ForeColor = Color.Red;
                simpleLabelItemErrorMsg.Text = errorMessage;
                return;
            }

            simpleLabelItemErrorMsg.ImageOptions.SvgImage = svgImageCollection1[0];
            simpleLabelItemErrorMsg.AppearanceItemCaption.ForeColor = Color.ForestGreen;
            simpleLabelItemErrorMsg.Text = "Validated";

            DialogResult = DialogResult.OK;
            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}