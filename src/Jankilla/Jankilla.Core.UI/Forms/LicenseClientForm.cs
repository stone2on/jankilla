using DevExpress.XtraEditors;
using Jankilla.Core.UI.Utils;
using Jankilla.Core.Utils;
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

namespace Jankilla.Core.UI.Forms
{
    public partial class LicenseClientForm : DevExpress.XtraEditors.XtraForm
    {
        public LicenseClientForm()
        {
            InitializeComponent();

            memoEditUserKey.Text = CryptoHelper.GenerateUserKey();
        }

        private void buttonActivate_Click(object sender, EventArgs e)
        {
            try
            {
                bool bActivated = CryptoHelper.ActivateLicense(memoEditUserKey.Text, memoEditSerialKey.Text);

                if (bActivated)
                {
                    simpleLabelItemActivateStatus.Text = "Activated";
                    simpleLabelItemActivateStatus.AppearanceItemCaption.ForeColor = Color.Green;
                    Trace.WriteLine("Activated");
                    label1.Text = "Activated";
                }
                else
                {
                    label1.Text = "Activation failed because the serial key does not match.";
                }
            }
            catch (Exception ex)
            {
                label1.Text = ex.Message;
            }
        }
    }
}