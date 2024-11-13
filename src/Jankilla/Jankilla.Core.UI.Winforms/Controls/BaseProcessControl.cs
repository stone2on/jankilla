using System;
using System.Drawing;
using System.Windows.Forms;

namespace Jankilla.Core.UI.WinForms.Controls
{
    public partial class BaseProcessControl : DevExpress.XtraEditors.XtraUserControl
    {
        #region Event Handler
        public virtual event EventHandler<EventArgs> ProcessClicked;
        #endregion

        #region Public Properties

        public string ProcessName { get; set; }
        public string ProcessID { get; set; }
        public string MachineStatus { get; set; }
        public Color Color { get; set; }
        public int ChannelCount { get; set; } = 1;
        public Image ProcessImage { get; set; }
        public string[] AlarmTexts { get; set; }
        #endregion

        #region Consts

        protected static readonly Font SmallSizeFont = new Font("Calibri", 7, FontStyle.Regular);
        protected static readonly Font SmallBoldFont = new Font("Calibri", 7, FontStyle.Bold);
        protected static readonly Font BoldFont = new Font("Calibri", 8, FontStyle.Bold);

        protected const string NOT_AVAILABLE = "N/A";

        #endregion

        #region Constructor
        public BaseProcessControl()
        {
            InitializeComponent();
        }
        #endregion

        #region Private Helpers

        private void DrawAlarms(Graphics g, int y)
        {
            g.FillRectangle(Brushes.DeepPink, 8, y + 3, 6, 6);
            g.DrawString("ALARMS", SmallSizeFont, Brushes.Black, 15, y);

            if (AlarmTexts == null)
            {
                return;
            }

            int nextY = y + 18;
            g.DrawLine(Pens.LightGray, 8, nextY, 8, nextY + ((AlarmTexts.Length - 1) * 15));
            int i;
            for (i = 0; i < AlarmTexts.Length; i++)
            {
                g.DrawLine(Pens.LightGray, 8, nextY + (15 * i), 11, nextY + (15 * i));
                g.DrawString(AlarmTexts[i], SmallSizeFont, Brushes.HotPink, 12, nextY - 5 + (15 * i));
            }

            //nextY = 80 + (15 * i);

        }

        #endregion

        #region Events

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;

            using (Brush machineBrush = new SolidBrush(Color))
            {
                // Name
                g.DrawRectangle(Pens.LightGray, 0, 0, Width - 1, Height - 1);
                g.FillRectangle(machineBrush, 8, 8, 8, 8);
                g.DrawString(ProcessName, this.Font, Brushes.Black, 18, 5);
                g.DrawString($"CH{ChannelCount:D2}", SmallSizeFont, Brushes.DimGray, Width - 28, 6);

                // Line
                g.DrawLine(Pens.LightGray, 5, 20, Width - 5, 20);

                // MW INFO
                if (ProcessImage != null)
                {
                    g.DrawImage(ProcessImage, 5, 23, 24, 24);
                }
                g.DrawString("CONTRACTOR", SmallSizeFont, Brushes.DimGray, 32, 24);
                g.DrawString(ProcessID, Font, Brushes.Black, 32, 34);

                g.DrawString("STATUS", SmallSizeFont, Brushes.DimGray, Width - 35, 24);


                string status = "RELEASE";
#if DEBUG
                status = "DEBUG";
#endif
                g.DrawString(status, SmallSizeFont, Brushes.DarkGray, Width - 39, 34);


                // Line
                g.DrawLine(Pens.LightGray, 10, 50, Width - 10, 50);

            }
        }

        protected virtual void OnMouseClick(object sender, MouseEventArgs e)
        {
            ProcessClicked?.Invoke(sender, EventArgs.Empty);
        }

        private void onMouseEnter(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Hand;
            BackColor = Color.FromArgb(100, 0xF5, 0xF8, 0xEA);
        }

        private void onMouseLeave(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Default;
            BackColor = Color.Transparent;
        }

       
        #endregion

    }
}
