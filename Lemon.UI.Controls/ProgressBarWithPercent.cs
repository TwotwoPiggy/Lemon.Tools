using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Lemon.UI.Controls
{
	public partial class ProgressBarWithPercent : ProgressBar
    {
        public Brush Brush { get; set; } = Brushes.WhiteSmoke;
        public Font PercentFont { get; set; }

		public ProgressBarWithPercent()
        {
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
            this.BackColor = ColorTranslator.FromHtml("#99ff99");
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Rectangle rect = ClientRectangle;
            Graphics g = e.Graphics;

            ProgressBarRenderer.DrawHorizontalBar(g, rect);
            rect.Inflate(-3, -3);
            if (Value > 0)
            {
                var clip = new Rectangle(rect.X, rect.Y, (int)((float)Value / Maximum * rect.Width), rect.Height);
                ProgressBarRenderer.DrawHorizontalChunks(g, clip);
            }

            string text = string.Format("{0}%", Value * 100 / Maximum); ;
            using (var font = new Font(FontFamily.GenericSerif, 20))
            {
                var sz = g.MeasureString(text, font);
                var location = new PointF(rect.Width / 2 - sz.Width / 2, rect.Height / 2 - sz.Height / 2 + 2);
                g.DrawString(text, font, Brush, location);
            }
        }
        
    }
}
