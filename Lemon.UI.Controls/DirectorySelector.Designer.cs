
namespace Lemon.UI.Controls
{
	partial class DirectorySelector
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Component Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			tb_Directory = new System.Windows.Forms.TextBox();
			btn_SelectDir = new System.Windows.Forms.Button();
			folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
			SuspendLayout();
			// 
			// tb_Directory
			// 
			tb_Directory.Location = new System.Drawing.Point(0, 4);
			tb_Directory.Margin = new System.Windows.Forms.Padding(0);
			tb_Directory.Name = "tb_Directory";
			tb_Directory.Size = new System.Drawing.Size(217, 27);
			tb_Directory.TabIndex = 0;
			// 
			// btn_SelectDir
			// 
			btn_SelectDir.Location = new System.Drawing.Point(221, 0);
			btn_SelectDir.Margin = new System.Windows.Forms.Padding(0);
			btn_SelectDir.Name = "btn_SelectDir";
			btn_SelectDir.Size = new System.Drawing.Size(100, 35);
			btn_SelectDir.TabIndex = 1;
			btn_SelectDir.Text = "选择路径";
			btn_SelectDir.UseVisualStyleBackColor = true;
			btn_SelectDir.Click += btn_SelectDir_Click;
			// 
			// DirectorySelector
			// 
			AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
			AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			Controls.Add(btn_SelectDir);
			Controls.Add(tb_Directory);
			Margin = new System.Windows.Forms.Padding(0);
			Name = "DirectorySelector";
			Size = new System.Drawing.Size(321, 35);
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion

		private System.Windows.Forms.TextBox tb_Directory;
		private System.Windows.Forms.Button btn_SelectDir;
		private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
	}
}
