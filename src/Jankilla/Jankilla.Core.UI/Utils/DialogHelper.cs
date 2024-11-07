using DevExpress.XtraBars.Docking2010.Customization;
using DevExpress.XtraBars.Docking2010.Views.WindowsUI;
using DevExpress.XtraEditors;
using DevExpress.XtraSplashScreen;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Jankilla.Core.UI.Utils
{
    public static class DialogHelper
    {
        public const string FILTER_STR_XLSX = "Excel Files (.xlsx)|*.xlsx";
        public const string FILTER_STR_CFS = "Feature Files (.cfs)|*.cfs";
        public const string FILTER_STR_CSV = "csv Files (.csv)|*.csv|All Files (*.*)|*.*";
        public const string FILTER_STR_PNG = "Png Files (.png)|*.png|All Files (*.*)|*.*";
        public const string FILTER_STR_ZIP = "Zip Files (.zip)|*.zip|All Files (*.*)|*.*";
        public const string FILTER_STR_PDF = "pdf Files (.pdf)|*.pdf|All Files (*.*)|*.*";
        public const string FILTER_STR_SCL = "scl Files (.scl)|*.scl|All Files (*.*)|*.*";
        public const string FILTER_STR_JSON = "JSON Files (.json)|*.json|All Files (*.*)|*.*";

        public static string ShowOpenFileDialog(string filterStr = null)
        {
            string filePath;

            using (var dialog = new OpenFileDialog())
            {
                if (filterStr != null)
                {
                    dialog.Filter = filterStr;
                }

                var result = dialog.ShowDialog();

                if (result != DialogResult.OK)
                {
                    return null;
                }

                filePath = dialog.FileName;
            }

            return File.Exists(filePath) ? filePath : null;
        }

        public static string ShowSaveFileDialog(string filterStr = null)
        {
            string filePath;

            using (var dialog = new SaveFileDialog())
            {
                if (filterStr != null)
                {
                    dialog.Filter = filterStr;
                }

                var result = dialog.ShowDialog();

                if (result != DialogResult.OK)
                {
                    return null;
                }

                filePath = dialog.FileName;
            }

            return filePath;
        }

        public static void ShowMessageBox(string message)
        {
            var args2 = new XtraMessageBoxArgs
            {
                Caption = "Confirm",
                Text = message
            };

            args2.Showing += (o, ex) => { ex.Form.MinimumSize = new Size(300, 70); };

            XtraMessageBox.Show(args2);
        }

        public static DialogResult ShowMessageBoxDialog(string message)
        {
            var args2 = new XtraMessageBoxArgs
            {
                Caption = "Confirm",
                Text = message,
                Buttons = new DialogResult[] { DialogResult.OK, DialogResult.Cancel }
            };

            args2.Showing += (o, ex) => { ex.Form.MinimumSize = new Size(300, 70); };

            return XtraMessageBox.Show(args2);
        }

       

        public static object ShowInputComboMessageBox<T>(string caption, string prompt) where T : Enum
        {
            var values = Enum.GetValues(typeof(T)).Cast<T>();
            return ShowInputComboMessageBox(caption, prompt, values);
        }

        public static object ShowInputComboMessageBox<T>(string caption, string prompt, IEnumerable<T> items)
        {
            if (items == null || items.Count() == 0)
            {
                throw new InvalidDataException();
            }

            XtraInputBoxArgs args = new XtraInputBoxArgs();
            args.Caption = caption;
            args.Prompt = prompt;
            args.DefaultButtonIndex = 0;
            args.Showing += (o, ex) =>
            {
                ex.Form.MinimumSize = new Size(300, 70);
            };

            ComboBoxEdit editor = new ComboBoxEdit();
            args.Editor = editor;

            foreach (var item in items)
            {
                editor.Properties.Items.Add(item);
            }
          
            editor.VisibleChanged += (o, ex) =>
            {
                if (editor.Visible)
                {
                    editor.SelectedIndex = 0;
                }
            };

            editor.ReadOnly = true;

            var result = XtraInputBox.Show(args);

            return result;
        }

        public static string ShowInputMessageBox(string content)
        {
            var args = new XtraInputBoxArgs();
            args.Caption = "Confirm";
            args.Prompt = content;
            args.DefaultResponse = string.Empty;

            var result = XtraInputBox.Show(args)?.ToString();

            return result;
        }

        public static async Task ShowMessageBoxWithBlinkingAsync(string message, TextEdit edit)
        {
            var args2 = new XtraMessageBoxArgs
            {
                Caption = "Confirm",
                Text = message
            };

            args2.Showing += (o, ex) => { ex.Form.MinimumSize = new Size(300, 70); };

            XtraMessageBox.Show(args2);

            for (int i = 0; i < 3; i++)
            {
                edit.BackColor = Color.Yellow;
                edit.Refresh();
                await Task.Delay(500);


                edit.BackColor = Color.Transparent;
                edit.Refresh();
                await Task.Delay(500);
            }

        }

        public static async Task ShowMessageBoxWithBlinkingAsync(string message, Control edit)
        {
            var args2 = new XtraMessageBoxArgs
            {
                Caption = "Confirm",
                Text = message
            };

            args2.Showing += (o, ex) => { ex.Form.MinimumSize = new Size(300, 70); };

            XtraMessageBox.Show(args2);

            for (int i = 0; i < 3; i++)
            {
                edit.BackColor = Color.Yellow;
                edit.Refresh();
                await Task.Delay(500);


                edit.BackColor = Color.White;
                edit.Refresh();
                await Task.Delay(500);
            }

        }


        public static void ShowPopupErrorMessage(string message, Form owner)
        {
            var action = new FlyoutAction() { Caption = "Abnormal behavior detected.", Description = message };
            var command1 = new FlyoutCommand() { Text = "OK", Result = DialogResult.Yes };

            action.Commands.Add(command1);

            var properties = new FlyoutProperties
            {
                ButtonSize = new Size(80, 25),
                Style = FlyoutStyle.Popup
            };

            FlyoutDialog.Show(owner, action, properties);
        }

        public static IOverlaySplashScreenHandle ShowProgressPanel(Form form)
        {
            IOverlaySplashScreenHandle handle = null;

            form.Invoke(new Action(() =>
            {
                handle = SplashScreenManager.ShowOverlayForm(form);
            }));

            return handle;
        }

        public static void CloseProgressPanel(IOverlaySplashScreenHandle handle)
        {
            if (handle != null)
                SplashScreenManager.CloseOverlayForm(handle);
        }

    }
}
