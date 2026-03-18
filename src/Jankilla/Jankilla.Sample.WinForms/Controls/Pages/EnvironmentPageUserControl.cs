using DevExpress.XtraEditors;
using Jankilla.Core.UI.WinForms.Utils;
using Jankilla.Sample.WinForms.Controls.Controllers;
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

namespace Jankilla.Sample.WinForms.Controls.Pages
{
    public partial class EnvironmentPageUserControl : DevExpress.XtraEditors.XtraUserControl
    {
        public EnvironmentPageUserControl()
        {
            InitializeComponent();
        }

        private void onLoad(object sender, EventArgs e)
        {
            propertyGridControlIniFile.SelectedObject = AccessManager.Instance;
        }

        private async void buttonEditLoadProjectFile_Click(object sender, EventArgs e)
        {
            var path = DialogHelper.ShowOpenFileDialog(DialogHelper.FILTER_STR_JSON);
            if (string.IsNullOrEmpty(path))
            {
                return;
            }

            buttonEditLoadProjectFile.Text = path;

            await AccessManager.Instance.LoadProjectAsync(path);
        }

        private async void buttonSync_Click(object sender, EventArgs e)
        {
            buttonSync.Enabled = false;
            layoutControlGroup2.Enabled = false;
            try
            {
                if (AccessManager.Instance.IsStarted)
                {
                    var result = DialogHelper.ShowMessageBoxDialog("The database synchronization operation cannot be performed during startup, " +
                                                                   "\ndo you want to pause the process now and run the synchronization operation?");

                    if (result == System.Windows.Forms.DialogResult.OK)
                    {
                        AccessManager.Instance.Cancel();
                        await AccessManager.Instance.SyncronizeProjectAsync();
                        AccessManager.Instance.Start();

                        return;
                    }
                }

                await AccessManager.Instance.SyncronizeProjectAsync();
            }
            catch (Exception ex)
            {
                Debug.Assert(false, ex.Message);
            }
            finally
            {
                buttonSync.Enabled = true;
                layoutControlGroup2.Enabled = true;
            }


        }

    }
}
