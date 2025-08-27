namespace Lemon.UI.Controls
{
	partial class FileSelector
	{
		/// <summary> 
		/// 必需的设计器变量。
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// 清理所有正在使用的资源。
		/// </summary>
		/// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region 组件设计器生成的代码

		/// <summary> 
		/// 设计器支持所需的方法 - 不要修改
		/// 使用代码编辑器修改此方法的内容。
		/// </summary>
		private void InitializeComponent()
		{
			tb_File = new System.Windows.Forms.TextBox();
			btn_SelectFile = new System.Windows.Forms.Button();
			openFileDialog = new System.Windows.Forms.OpenFileDialog();
			SuspendLayout();
			// 
			// tb_File
			// 
			tb_File.Location = new System.Drawing.Point(0, 4);
			tb_File.Margin = new System.Windows.Forms.Padding(0);
			tb_File.Name = "tb_File";
			tb_File.Size = new System.Drawing.Size(217, 27);
			tb_File.TabIndex = 0;
			// 
			// btn_SelectFile
			// 
			btn_SelectFile.Location = new System.Drawing.Point(221, 0);
			btn_SelectFile.Margin = new System.Windows.Forms.Padding(0);
			btn_SelectFile.Name = "btn_SelectFile";
			btn_SelectFile.Size = new System.Drawing.Size(100, 35);
			btn_SelectFile.TabIndex = 1;
			btn_SelectFile.Text = "选择文件";
			btn_SelectFile.UseVisualStyleBackColor = true;
			btn_SelectFile.Click += btn_SelectFile_Click;
			// 
			// openFileDialog
			// 
			openFileDialog.FileName = "openFileDialog1";
			// 
			// FileSelector
			// 
			AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
			AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			Controls.Add(btn_SelectFile);
			Controls.Add(tb_File);
			Name = "FileSelector";
			Size = new System.Drawing.Size(321, 35);
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion

		private System.Windows.Forms.TextBox tb_File;
		private System.Windows.Forms.Button btn_SelectFile;
		private System.Windows.Forms.OpenFileDialog openFileDialog;
	}
}
