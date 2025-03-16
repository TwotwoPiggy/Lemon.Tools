namespace ReaLTaiizorTest
{
	partial class Form1
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

		#region Windows 窗体设计器生成的代码

		/// <summary>
		/// 设计器支持所需的方法 - 不要修改
		/// 使用代码编辑器修改此方法的内容。
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
			this.parrotForm1 = new ReaLTaiizor.Forms.ParrotForm();
			this.parrotButton1 = new ReaLTaiizor.Controls.ParrotButton();
			this.parrotForm1.WorkingArea.SuspendLayout();
			this.parrotForm1.SuspendLayout();
			this.SuspendLayout();
			// 
			// parrotForm1
			// 
			this.parrotForm1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(236)))), ((int)(((byte)(236)))));
			this.parrotForm1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.parrotForm1.ExitApplication = true;
			this.parrotForm1.FormStyle = ReaLTaiizor.Forms.ParrotForm.Style.MacOS;
			this.parrotForm1.Location = new System.Drawing.Point(0, 0);
			this.parrotForm1.MacOSForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
			this.parrotForm1.MacOSLeftBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
			this.parrotForm1.MacOSRightBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(210)))), ((int)(((byte)(210)))), ((int)(((byte)(210)))));
			this.parrotForm1.MacOSSeparatorColor = System.Drawing.Color.FromArgb(((int)(((byte)(173)))), ((int)(((byte)(173)))), ((int)(((byte)(173)))));
			this.parrotForm1.MaterialBackColor = System.Drawing.Color.DodgerBlue;
			this.parrotForm1.MaterialForeColor = System.Drawing.Color.White;
			this.parrotForm1.Name = "parrotForm1";
			this.parrotForm1.ShowMaximize = true;
			this.parrotForm1.ShowMinimize = true;
			this.parrotForm1.Size = new System.Drawing.Size(800, 450);
			this.parrotForm1.SmoothingType = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
			this.parrotForm1.TabIndex = 0;
			this.parrotForm1.TitleText = "Parrot Form";
			this.parrotForm1.UbuntuForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(210)))));
			this.parrotForm1.UbuntuLeftBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(85)))), ((int)(((byte)(80)))));
			this.parrotForm1.UbuntuRightBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(65)))), ((int)(((byte)(60)))));
			// 
			// parrotForm1.WorkingArea
			// 
			this.parrotForm1.WorkingArea.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(236)))), ((int)(((byte)(236)))));
			this.parrotForm1.WorkingArea.Controls.Add(this.parrotButton1);
			this.parrotForm1.WorkingArea.Dock = System.Windows.Forms.DockStyle.Fill;
			this.parrotForm1.WorkingArea.Location = new System.Drawing.Point(0, 39);
			this.parrotForm1.WorkingArea.Name = "WorkingArea";
			this.parrotForm1.WorkingArea.Size = new System.Drawing.Size(800, 411);
			this.parrotForm1.WorkingArea.TabIndex = 0;
			// 
			// parrotButton1
			// 
			this.parrotButton1.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
			this.parrotButton1.ButtonImage = ((System.Drawing.Image)(resources.GetObject("parrotButton1.ButtonImage")));
			this.parrotButton1.ButtonStyle = ReaLTaiizor.Controls.ParrotButton.Style.MaterialRounded;
			this.parrotButton1.ButtonText = "Button";
			this.parrotButton1.ClickBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(195)))), ((int)(((byte)(195)))), ((int)(((byte)(195)))));
			this.parrotButton1.ClickTextColor = System.Drawing.Color.DodgerBlue;
			this.parrotButton1.CornerRadius = 5;
			this.parrotButton1.Cursor = System.Windows.Forms.Cursors.Hand;
			this.parrotButton1.Horizontal_Alignment = System.Drawing.StringAlignment.Center;
			this.parrotButton1.HoverBackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(225)))), ((int)(((byte)(225)))));
			this.parrotButton1.HoverTextColor = System.Drawing.Color.DodgerBlue;
			this.parrotButton1.ImagePosition = ReaLTaiizor.Controls.ParrotButton.ImgPosition.Left;
			this.parrotButton1.Location = new System.Drawing.Point(164, 116);
			this.parrotButton1.Name = "parrotButton1";
			this.parrotButton1.Size = new System.Drawing.Size(200, 50);
			this.parrotButton1.SmoothingType = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
			this.parrotButton1.TabIndex = 0;
			this.parrotButton1.TextColor = System.Drawing.Color.DodgerBlue;
			this.parrotButton1.TextRenderingType = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
			this.parrotButton1.Vertical_Alignment = System.Drawing.StringAlignment.Center;
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(800, 450);
			this.Controls.Add(this.parrotForm1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Name = "Form1";
			this.Text = "Form1";
			this.parrotForm1.WorkingArea.ResumeLayout(false);
			this.parrotForm1.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private ReaLTaiizor.Forms.ParrotForm parrotForm1;
		private ReaLTaiizor.Controls.ParrotButton parrotButton1;
	}
}

