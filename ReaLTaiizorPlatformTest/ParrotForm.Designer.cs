namespace ReaLTaiizorPlatformTest
{
	partial class ParrotForm
	{
		/// <summary>
		///  Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		///  Clean up any resources being used.
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

		#region Windows Form Designer generated code

		/// <summary>
		///  Required method for Designer support - do not modify
		///  the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.parrotForm1 = new ReaLTaiizor.Forms.ParrotForm();
			this.airButton1 = new ReaLTaiizor.Controls.AirButton();
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
			this.parrotForm1.Size = new System.Drawing.Size(802, 529);
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
			this.parrotForm1.WorkingArea.Controls.Add(this.airButton1);
			this.parrotForm1.WorkingArea.Dock = System.Windows.Forms.DockStyle.Fill;
			this.parrotForm1.WorkingArea.Location = new System.Drawing.Point(0, 39);
			this.parrotForm1.WorkingArea.Name = "WorkingArea";
			this.parrotForm1.WorkingArea.Size = new System.Drawing.Size(802, 490);
			this.parrotForm1.WorkingArea.TabIndex = 0;
			// 
			// airButton1
			// 
			this.airButton1.Cursor = System.Windows.Forms.Cursors.Hand;
			this.airButton1.Customization = "7e3t//Ly8v/r6+v/5ubm/+vr6//f39//p6en/zw8PP8UFBT/gICA/w==";
			this.airButton1.Font = new System.Drawing.Font("Segoe UI", 9F);
			this.airButton1.Image = null;
			this.airButton1.Location = new System.Drawing.Point(95, 142);
			this.airButton1.Name = "airButton1";
			this.airButton1.NoRounding = false;
			this.airButton1.Size = new System.Drawing.Size(100, 45);
			this.airButton1.TabIndex = 0;
			this.airButton1.Text = "airButton1";
			this.airButton1.Transparent = false;
			// 
			// ParrotForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoScroll = true;
			this.ClientSize = new System.Drawing.Size(802, 529);
			this.Controls.Add(this.parrotForm1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Name = "ParrotForm";
			this.Text = "Form1";
			this.parrotForm1.WorkingArea.ResumeLayout(false);
			this.parrotForm1.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private ReaLTaiizor.Forms.ParrotForm parrotForm1;
		private ReaLTaiizor.Controls.AirButton airButton1;
	}
}
