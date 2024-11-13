using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Jankilla.Core.UI.WinForms.Controls
{
    public partial class BaseDeviceControl : DevExpress.XtraEditors.XtraUserControl
    {
        public string DeviceName { get; set; }
        public Color Color { get; set; }
        public Color HoverColor { get; set; }
        public bool Connected { get; set; }
        public Image DeviceImage { get; set; }
        public string Description { get; set; } = string.Empty;
        public BaseDeviceControl()
        {
            InitializeComponent();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics g = e.Graphics;

            using (Brush machineBrush = new SolidBrush(Color))
            using (Brush penBrush = new SolidBrush(ForeColor))
            {
                // Name
                g.DrawRectangle(Pens.LightGray, 0, 0, Width - 1, Height - 1);
                g.FillRectangle(machineBrush, 8, 8, 8, 8);
                g.DrawString(DeviceName, this.Font, penBrush, 18, 5);

                // Line
                g.DrawLine(Pens.LightGray, 5, 20, Width - 5, 20);

                if (DeviceImage != null)
                {
                    g.DrawImage(DeviceImage, 5, 26, 24, 24);
                }

                if (Description != null)
                {
                    g.DrawString(Description, this.Font, penBrush, 35, 25);
                }

                Brush brush = Connected ? Brushes.LightGreen : Brushes.Red;

                g.FillRectangle(brush, 35, 43, Width - 50, 6);

            }
        }

 

        private void onMouseEnter(object sender, EventArgs e)
        {
            //this.Cursor = Cursors.Hand;
            //BackColor = Color.FromArgb(100, 0xF5, 0xF8, 0xEA);
            BackColor = HoverColor;
        }

        private void onMouseLeave(object sender, EventArgs e)
        {
            //this.Cursor = Cursors.Default;
            BackColor = Color.Transparent;
        }

    }
}
