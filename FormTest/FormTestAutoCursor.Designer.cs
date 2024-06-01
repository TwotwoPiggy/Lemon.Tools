
namespace FormTest
{
	partial class FormTestAutoCursor
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
			this.components = new System.ComponentModel.Container();
			this.btnMoveCursor = new System.Windows.Forms.Button();
			this.button1 = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			this.button3 = new System.Windows.Forms.Button();
			this.button4 = new System.Windows.Forms.Button();
			this.button5 = new System.Windows.Forms.Button();
			this.button6 = new System.Windows.Forms.Button();
			this.smoothProgressBar1 = new Lemon.UI.Controls.SmoothProgressBar();
			this.timer1 = new System.Windows.Forms.Timer(this.components);
			this.SuspendLayout();
			// 
			// btnMoveCursor
			// 
			this.btnMoveCursor.Location = new System.Drawing.Point(12, 12);
			this.btnMoveCursor.Name = "btnMoveCursor";
			this.btnMoveCursor.Size = new System.Drawing.Size(147, 35);
			this.btnMoveCursor.TabIndex = 0;
			this.btnMoveCursor.Text = "MoveCursor";
			this.btnMoveCursor.UseVisualStyleBackColor = true;
			this.btnMoveCursor.Click += new System.EventHandler(this.btnMoveCursor_Click);
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(224, 12);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(93, 35);
			this.button1.TabIndex = 1;
			this.button1.Text = "Screenshot";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// button2
			// 
			this.button2.Location = new System.Drawing.Point(22, 93);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(106, 49);
			this.button2.TabIndex = 2;
			this.button2.Text = "gpt";
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new System.EventHandler(this.button2_Click);
			// 
			// button3
			// 
			this.button3.Location = new System.Drawing.Point(224, 97);
			this.button3.Name = "button3";
			this.button3.Size = new System.Drawing.Size(97, 45);
			this.button3.TabIndex = 3;
			this.button3.Text = "shutdown";
			this.button3.UseVisualStyleBackColor = true;
			this.button3.Click += new System.EventHandler(this.button3_Click);
			// 
			// button4
			// 
			this.button4.Location = new System.Drawing.Point(22, 198);
			this.button4.Name = "button4";
			this.button4.Size = new System.Drawing.Size(121, 54);
			this.button4.TabIndex = 4;
			this.button4.Text = "cancelProcess";
			this.button4.UseVisualStyleBackColor = true;
			this.button4.Click += new System.EventHandler(this.button4_Click);
			// 
			// button5
			// 
			this.button5.Location = new System.Drawing.Point(224, 203);
			this.button5.Name = "button5";
			this.button5.Size = new System.Drawing.Size(113, 45);
			this.button5.TabIndex = 5;
			this.button5.Text = "renameFile";
			this.button5.UseVisualStyleBackColor = true;
			this.button5.Click += new System.EventHandler(this.button5_Click);
			// 
			// button6
			// 
			this.button6.Location = new System.Drawing.Point(31, 281);
			this.button6.Name = "button6";
			this.button6.Size = new System.Drawing.Size(75, 23);
			this.button6.TabIndex = 6;
			this.button6.Text = "button6";
			this.button6.UseVisualStyleBackColor = true;
			this.button6.Click += new System.EventHandler(this.button6_Click);
			// 
			// smoothProgressBar1
			// 
			this.smoothProgressBar1.Location = new System.Drawing.Point(59, 354);
			this.smoothProgressBar1.Maximum = 100;
			this.smoothProgressBar1.Minimum = 0;
			this.smoothProgressBar1.Name = "smoothProgressBar1";
			this.smoothProgressBar1.ProgressBarColor = System.Drawing.ColorTranslator.FromHtml("#99ff99");
			this.smoothProgressBar1.Size = new System.Drawing.Size(181, 30);
			this.smoothProgressBar1.TabIndex = 7;
			this.smoothProgressBar1.Value = 0;
			//this.smoothProgressBar1.
			// 
			// timer1
			// 
			this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
			// 
			// FormTestAutoCursor
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnMoveCursor;
			this.ClientSize = new System.Drawing.Size(465, 539);
			this.Controls.Add(this.smoothProgressBar1);
			this.Controls.Add(this.button6);
			this.Controls.Add(this.button5);
			this.Controls.Add(this.button4);
			this.Controls.Add(this.button3);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.btnMoveCursor);
			this.Name = "FormTestAutoCursor";
			this.Text = "FormTestAutoCursor";
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button btnMoveCursor;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.Button button3;
		private System.Windows.Forms.Button button4;
		private System.Windows.Forms.Button button5;
		private System.Windows.Forms.Button button6;
		private Lemon.UI.Controls.SmoothProgressBar smoothProgressBar1;
		private System.Windows.Forms.Timer timer1;
	}
}

