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
	public partial class FileSelector : UserControl
	{
		public override string Text
		{
			get
			{
				return this.tb_File.Text;
			}
			set
			{
				this.tb_File.Text = value;
			}
		}

		public string ButtonText
		{
			get
			{
				return this.btn_SelectFile.Text;
			}
			set
			{
				this.btn_SelectFile.Text = value;
			}
		}

		public Size ButtonSize
		{
			get
			{
				return this.btn_SelectFile.Size;
			}
			set
			{
				this.btn_SelectFile.Size = value;
			}
		}

		public FileSelector()
		{
			InitializeComponent();
		}

		private void btn_SelectFile_Click(object sender, EventArgs e)
		{
			if (this.openFileDialog.ShowDialog() == DialogResult.OK)
			{
				this.Text = this.openFileDialog.FileName;
			}
		}
	}
}
