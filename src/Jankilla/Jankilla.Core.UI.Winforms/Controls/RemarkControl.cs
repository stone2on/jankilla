using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Jankilla.Core.UI.WinForms.Controls
{
    public partial class RemarkControl : DevExpress.XtraEditors.XtraUserControl
    {
        public List<Remark> Remarks { get; } = new List<Remark>();

        public int ColumnWidth { get; set; } = 100;
        public int MaxRowCount { get; set; } = 5;

        #region Constructor

        public RemarkControl()
        {
            InitializeComponent();
        }

        #endregion

        #region Commons

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics g = e.Graphics;

            int x = 2;
            int y = 5;

            int height = 12;

            int cnt = 0;
            foreach (var remark in Remarks)
            {
                using (Brush br = new SolidBrush(remark.Color))
                {
                    g.FillRectangle(br, x, y, 8, 8);
                }

                g.DrawString(remark.Name, this.Font, Brushes.Black, x + 10, y - 3);

                y += height;

                ++cnt;

                if (cnt == MaxRowCount)
                {
                    x += ColumnWidth;
                    y = 5;
                    cnt = 0;
                }
            }
        }

        #endregion
    }

    public class Remark
    {
        public string Name { get;}
        public Color Color { get;}
        public Remark(string name, Color color)
        {
            Name = name ?? throw new ArgumentNullException();
            Color = color;
        }
    }

}
