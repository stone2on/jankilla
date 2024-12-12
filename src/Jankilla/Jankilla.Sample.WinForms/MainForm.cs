using Jankilla.Core.Utils;
using Jankilla.Sample.WinForms.Controls.Controllers;
using Jankilla.Core.UI.WinForms.Listeners;
using Jankilla.Core.UI.WinForms.Controls;
using Jankilla.Core.UI.WinForms.Utils;
using Jankilla.Core.UI.WinForms.Forms;
using System;
using System.Drawing;
using System.IO;
using System.Text;
using System.Timers;
using System.Windows.Forms;
using System.Diagnostics;

namespace Jankilla.Sample.WinForms
{
    public partial class MainForm : DevExpress.XtraEditors.XtraForm
    {
        private static System.Timers.Timer exitTimer;

        #region Constructor

        public MainForm()
        {
            InitializeComponent();
#if !DEBUG
            LoadLicense();
#endif
        }

        #endregion

        #region Private Helpers
        private void LoadLicense()
        {
            if (CryptoHelper.IsActivated)
            {
                return;
            }

            using (var LicenseClientForm = new LicenseClientForm())
            {
                LicenseClientForm.IconOptions.Icon = this.IconOptions.Icon;
                LicenseClientForm.ShowDialog();
            }

            if (CryptoHelper.IsActivated == false)
            {
                DialogHelper.ShowMessageBox("License file not found. Please provide a valid license. The trial version is only available for 30 minutes.");
                Text = $"{Text} (Not Activated)";
                StartExitTimer();
            }

        }

        private void StartExitTimer()
        {

            exitTimer = new System.Timers.Timer(30 * 60 * 1000);
            exitTimer.Elapsed += OnTimedEvent;
            exitTimer.AutoReset = false; 
            exitTimer.Enabled = true;
        }

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            MessageBox.Show("The application will now close because a valid license was not found.", "License Expired", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            Application.Exit();
        }


        #endregion

        #region Events

        private void onLoad(object sender, EventArgs e)
        {
            var listener = new ListBoxTraceListener(listBoxLog);
            Trace.Listeners.Add(listener);

            remarkControl1.Remarks.Add(new Remark("Not Available", Color.Gray));
            remarkControl1.Remarks.Add(new Remark("Disconnected", Color.Red));
            remarkControl1.Remarks.Add(new Remark("Connected", Color.LightGreen));

            AccessManager.Instance.DBStatusChanged += accessManager_DBStatusChanged;
            AccessManager.Instance.PLCStatusChanged += accessManager_PLCStatusChanged;
            AccessManager.Instance.ProcessStatusChanged += accessManager_ProcessStatusChanged;

            if (AccessManager.Instance.AutoRun)
            {
                Trace.WriteLine("Process autorun is enabled. Let's start the process!");
                AccessManager.Instance.Start();
            }

            baseDeviceControlDB.Connected = AccessManager.Instance.IsDBOpened;
        }

        private void accessManager_ProcessStatusChanged(object sender, EventArgs e)
        {
            bool bStarted = AccessManager.Instance.IsStarted;

            baseDeviceControlProcess.Connected = bStarted;
            buttonProcessStart.Enabled = !bStarted;
            buttonProcessStop.Enabled = bStarted;

            if (bStarted)
            {
                baseDeviceControlProcess.Description = "Started";
            }
            else
            {
                baseDeviceControlProcess.Description = "Stopped";
            }
        }

        private void accessManager_PLCStatusChanged(object sender, EventArgs e)
        {
            baseDeviceControlPLC.Connected = AccessManager.Instance.IsPLCOpened;
        }

        private void accessManager_DBStatusChanged(object sender, EventArgs e)
        {
            baseDeviceControlDB.Connected = AccessManager.Instance.IsDBOpened;
        }

        private void accordionControlElementDashBoard_Click(object sender, EventArgs e)
        {
            navigationFrameMain.SelectedPage = navigationPageDashboard;
        }

        private void accordionControlElementHistory_Click(object sender, EventArgs e)
        {
            navigationFrameMain.SelectedPage = navigationPageHistory;
        }

        private void accordionControlElementData_Click(object sender, EventArgs e)
        {
            navigationFrameMain.SelectedPage = navigationPageSetting;
        }

        private void barButtonItemDashoboard_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            navigationFrameMain.SelectedPage = navigationPageDashboard;
        }

        private void barButtonItemSetting_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            navigationFrameMain.SelectedPage = navigationPageSetting;
        }

        private void barButtonItemHistory_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            navigationFrameMain.SelectedPage = navigationPageHistory;
        }

        private void listBoxLog_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.Control == true && e.KeyCode == Keys.C)
            {
                var sb = new StringBuilder();
                foreach (var item in listBoxLog.SelectedItems)
                {
                    sb.AppendLine(item.ToString());
                }
                Clipboard.SetData(DataFormats.StringFormat, sb.ToString());  
            }
        }

        private void buttonProcessStart_Click(object sender, EventArgs e)
        {
            Trace.WriteLine("The user clicked the Start command.");
            AccessManager.Instance.Start();
        }

        private void buttonProcessStop_Click(object sender, EventArgs e)
        {
            Trace.WriteLine("The user clicked the Stop command.");
            AccessManager.Instance.Cancel();
        }

        private void barButtonItemCommandStart_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Trace.WriteLine("The user clicked the Start command.");
            AccessManager.Instance.Start();
        }

        private void barButtonItemCommandStop_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Trace.WriteLine("The user clicked the Stop command.");
            AccessManager.Instance.Cancel();
        }

        #endregion


    }
}