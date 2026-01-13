using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lemon.UI.Controls
{
	public partial class LabelTextbox : UserControl
	{
        #region labels controller
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public bool EnableLeftLabel
		{
			get
			{
				return leftLabel.Visible;
			}
			set
			{
				leftLabel.Visible = value;
				ControlLayout();
			}
		}
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public bool EnableRightLabel
		{
			get
			{
				return rightLabel.Visible;
			}
			set
			{
				rightLabel.Visible = value;
				ControlLayout();
			}
		}
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public bool EnableBothLabels
		{
			get
			{
				return leftLabel.Enabled && rightLabel.Enabled;
			}
			set
			{
				leftLabel.Visible = rightLabel.Visible = value;
				ControlLayout();
			}
		}
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public string LeftLabelText
		{
			get
			{
				return leftLabel.Text;
			}
			set
			{
				leftLabel.Text = value;
			}
		}
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public string RightLabelText
		{
			get
			{
				return rightLabel.Text;
			}
			set
			{
				rightLabel.Text = value;
			}
		}
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public Size TextBoxSize
		{
			get
			{
				return textBox.Size;
			}
			set
			{
				textBox.Size = value;
			}
		}
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public string Content
		{
			get
			{
				return textBox.Text;
			}
			set
			{
				textBox.Text = value;
			}
		}
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public TextBox TextBox
		{
			get
			{
				return textBox;
			}
			set
			{
				textBox = value;
			}
		}
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public Label LeftLabel
		{
			get
			{
				return leftLabel;
			}
			set
			{
				leftLabel = value;
			}
		}
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public Label RightLabel
		{
			get
			{
				return rightLabel;
			}
			set
			{
				rightLabel = value;
			}
		}
		#endregion


		public LabelTextbox()
		{
			InitializeComponent();
			ControlLayout();
		}

		public override void Refresh()
		{
			ControlLayout();
			base.Refresh();
		}

		private void ControlLayout()
		{
			var totalWidth = 100F;
			var labelWidth = 30F;
			if (leftLabel.Visible)
			{
				layout.ColumnStyles[0] = new ColumnStyle(SizeType.Percent, labelWidth);
				layout.ColumnStyles[1] = new ColumnStyle(SizeType.Percent, rightLabel.Enabled ? totalWidth - labelWidth * 2 : totalWidth - labelWidth);

			}
			if (rightLabel.Visible)
			{
				layout.ColumnStyles[2] = new ColumnStyle(SizeType.Percent, labelWidth);
			}
		}

	}
}
