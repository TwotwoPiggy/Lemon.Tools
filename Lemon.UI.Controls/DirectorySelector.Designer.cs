
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
			this.tb_Directory = new System.Windows.Forms.TextBox();
			this.btn_SelectDir = new System.Windows.Forms.Button();
			this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
			this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
			this.SuspendLayout();
			// 
			// tb_Directory
			// 
			this.tb_Directory.Location = new System.Drawing.Point(0, 3);
			this.tb_Directory.Margin = new System.Windows.Forms.Padding(0);
			this.tb_Directory.Name = "tb_Directory";
			//this.tb_Directory.AutoSize = false;
			this.tb_Directory.Size = new System.Drawing.Size(170, 30);
			this.tb_Directory.TabIndex = 0;
			// 
			// btn_SelectDir
			// 
			this.btn_SelectDir.Location = new System.Drawing.Point(172, 0);
			this.btn_SelectDir.Margin = new System.Windows.Forms.Padding(0);
			this.btn_SelectDir.Name = "btn_SelectDir";
			//this.btn_SelectDir.AutoSize = false;
			this.btn_SelectDir.Size = new System.Drawing.Size(78, 30);
			this.btn_SelectDir.TabIndex = 1;
			this.btn_SelectDir.Text = "选择路径";
			this.btn_SelectDir.UseVisualStyleBackColor = true;
			this.btn_SelectDir.Click += new System.EventHandler(this.btn_SelectDir_Click);
			// 
			// DirectorySelector
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.btn_SelectDir);
			this.Controls.Add(this.tb_Directory);
			this.Margin = new System.Windows.Forms.Padding(0);
			this.Name = "DirectorySelector";
			this.Size = new System.Drawing.Size(250, 30);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox tb_Directory;
		private System.Windows.Forms.Button btn_SelectDir;
		private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
		private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
	}
}
