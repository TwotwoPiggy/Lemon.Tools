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
