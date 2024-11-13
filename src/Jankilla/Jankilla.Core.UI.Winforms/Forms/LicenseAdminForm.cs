using DevExpress.XtraEditors;
using Jankilla.Core.UI.WinForms.Utils;
using Jankilla.Core.Utils;
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
    public partial class LicenseAdminForm : DevExpress.XtraEditors.XtraForm
    {
        public LicenseAdminForm()
        {
            InitializeComponent();
        }

        private void buttonGenerate_Click(object sender, EventArgs e)
        {
            try
            {
                memoEditSerialKey.Text = CryptoHelper.GenerateSerialKey(memoEditUserKey.Text);
            }
            catch (Exception ex) {
                DialogHelper.ShowMessageBoxDialog(ex.Message);
            }
        }
    }
}