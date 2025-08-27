using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Lemon.UI.Controls
{
	public partial class SwitchButton : UserControl
	{
		#region properties
		private bool _isChecked;
        public bool IsChecked 
		{
			get { return _isChecked; }
			set
			{
				_isChecked = value;
				Invalidate();
			} 
		}

		private Color _openColor = Color.Green;
        public Color OpenColor 
		{
			get { return _openColor; }
			set
			{
				_openColor = value;
				Invalidate();
			}
		}

		private Color _closeColor = Color.Gray;
		public Color CloseColor
		{
			get { return _closeColor; }
			set
			{
				_closeColor = value;
				Invalidate();
			}
		}

		private int _cornerRadius = 5;
        public int CornerRadius 
		{
			get { return _cornerRadius; }
			set
			{
				_cornerRadius = value;
				Invalidate();
			}
		}

		private int _borderWidth = 2;
        public int BorderWidth 
		{
			get { return _borderWidth; }
			set
			{
				_borderWidth = value;
				Invalidate();
			}
		}

        #endregion

        public SwitchButton()
		{
			InitializeComponent();
			SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			SetStyle(ControlStyles.UserPaint, true);
			SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
			SetStyle(ControlStyles.ResizeRedraw, true);
			SetStyle(ControlStyles.SupportsTransparentBackColor, true);
			this.Click += SwitchButton_Click;
		}


		private void SwitchButton_Click(object sender, EventArgs e)
		{
			_isChecked = !_isChecked;
			Invalidate();
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			//获取绘图对象
			Graphics g = e.Graphics;
			//呈现质量设置为高质量
			g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

			//先绘制整体轮廓
			GraphicsPath path = new GraphicsPath();
			Color fillColor = IsChecked ? OpenColor : CloseColor;

			path.AddArc(new Rectangle(0, 0, CornerRadius * 2, CornerRadius * 2), 180, 90);
			path.AddArc(new Rectangle(Width - 1 - CornerRadius * 2, 0, CornerRadius * 2, CornerRadius * 2), -90, 90);
			path.AddArc(new Rectangle(Width - 1 - CornerRadius * 2, Height - 1 - CornerRadius * 2, CornerRadius * 2, CornerRadius * 2), 0, 90);
			path.AddArc(new Rectangle(0, Height - 1 - CornerRadius * 2, CornerRadius * 2, CornerRadius * 2), 90, 90);
			path.CloseFigure();
			g.FillPath(new SolidBrush(fillColor), path);

			//再填充内部形状
			if (IsChecked)
			{
				GraphicsPath truePath = new GraphicsPath();
				truePath.AddArc(new Rectangle(Width >> 1, BorderWidth, CornerRadius * 2 - BorderWidth * 2, CornerRadius * 2 - BorderWidth * 2), 180, 90);
				truePath.AddArc(new Rectangle(Width - 1 + BorderWidth - CornerRadius * 2, BorderWidth, CornerRadius * 2 - BorderWidth * 2, CornerRadius * 2 - BorderWidth * 2), -90, 90);
				truePath.AddArc(new Rectangle(Width - 1 + BorderWidth - CornerRadius * 2, Height - 1 + BorderWidth - CornerRadius * 2, CornerRadius * 2 - BorderWidth * 2, CornerRadius * 2 - BorderWidth * 2), 0, 90);
				truePath.AddArc(new Rectangle(Width / 2, Height - 1 + BorderWidth - CornerRadius * 2, CornerRadius * 2 - BorderWidth * 2, CornerRadius * 2 - BorderWidth * 2), 90, 90);

				g.FillPath(new SolidBrush(Color.White), truePath);
				//中心点
				int x1 = 3 * Width / 4;
				int y1 = Height / 2;
				//半径
				int r = Height / 4;
				g.DrawEllipse(new Pen(OpenColor, BorderWidth), new Rectangle(x1 - r, y1 - r, 2 * r, 2 * r));
			}
			else
			{
				GraphicsPath truePath = new GraphicsPath();
				truePath.AddArc(new Rectangle(BorderWidth, BorderWidth, CornerRadius * 2 - BorderWidth * 2, CornerRadius * 2 - BorderWidth * 2), 180, 90);
				truePath.AddArc(new Rectangle(Width / 2 + BorderWidth - CornerRadius * 2, BorderWidth, CornerRadius * 2 - BorderWidth * 2, CornerRadius * 2 - BorderWidth * 2), -90, 90);
				truePath.AddArc(new Rectangle(Width / 2 + BorderWidth - CornerRadius * 2, Height - 1 + BorderWidth - CornerRadius * 2, CornerRadius * 2 - BorderWidth * 2, CornerRadius * 2 - BorderWidth * 2), 0, 90);
				truePath.AddArc(new Rectangle(BorderWidth, Height - 1 + BorderWidth - CornerRadius * 2, CornerRadius * 2 - BorderWidth * 2, CornerRadius * 2 - BorderWidth * 2), 90, 90);

				g.FillPath(new SolidBrush(Color.White), truePath);
				//中心点
				int x1 = Width / 4;
				int y1 = Height / 2;
				//半径
				int r = Height / 4;
				g.DrawEllipse(new Pen(CloseColor, BorderWidth), new Rectangle(x1 - r, y1 - r, 2 * r, 2 * r));
			}
		}
	}
}
