using System;
using System.Windows.Forms;

namespace Lemon.UI.Controls
{
	public partial class DirectorySelector : UserControl
	{
		public override string Text 
		{
			get
			{
				return this.tb_Directory.Text;
			}
			set
			{
				this.tb_Directory.Text = value;
			}
		}

		public string ButtonText
		{
			get
			{
				return this.btn_SelectDir.Text;
			}
			set
			{
				this.btn_SelectDir.Text = value;
			}
		}
		public DirectorySelector()
		{
			InitializeComponent();
		}

		private void btn_SelectDir_Click(object sender, EventArgs e)
		{
			this.folderBrowserDialog.Description = "请选择文件路径";
			if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
			{
				this.tb_Directory.Text = this.folderBrowserDialog.SelectedPath;
			}
		}
	}
}
