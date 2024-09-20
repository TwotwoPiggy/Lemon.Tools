
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
			components = new System.ComponentModel.Container();
			btnMoveCursor = new System.Windows.Forms.Button();
			button1 = new System.Windows.Forms.Button();
			button2 = new System.Windows.Forms.Button();
			button3 = new System.Windows.Forms.Button();
			button4 = new System.Windows.Forms.Button();
			button5 = new System.Windows.Forms.Button();
			button6 = new System.Windows.Forms.Button();
			smoothProgressBar1 = new Lemon.UI.Controls.SmoothProgressBar();
			timer1 = new System.Windows.Forms.Timer(components);
			button7 = new System.Windows.Forms.Button();
			button8 = new System.Windows.Forms.Button();
			richTextBox1 = new System.Windows.Forms.RichTextBox();
			SuspendLayout();
			// 
			// btnMoveCursor
			// 
			btnMoveCursor.Location = new System.Drawing.Point(12, 12);
			btnMoveCursor.Name = "btnMoveCursor";
			btnMoveCursor.Size = new System.Drawing.Size(147, 35);
			btnMoveCursor.TabIndex = 0;
			btnMoveCursor.Text = "MoveCursor";
			btnMoveCursor.UseVisualStyleBackColor = true;
			btnMoveCursor.Click += btnMoveCursor_Click;
			// 
			// button1
			// 
			button1.Location = new System.Drawing.Point(224, 12);
			button1.Name = "button1";
			button1.Size = new System.Drawing.Size(93, 35);
			button1.TabIndex = 1;
			button1.Text = "Screenshot";
			button1.UseVisualStyleBackColor = true;
			button1.Click += button1_Click;
			// 
			// button2
			// 
			button2.Location = new System.Drawing.Point(22, 93);
			button2.Name = "button2";
			button2.Size = new System.Drawing.Size(106, 49);
			button2.TabIndex = 2;
			button2.Text = "gpt";
			button2.UseVisualStyleBackColor = true;
			button2.Click += button2_Click;
			// 
			// button3
			// 
			button3.Location = new System.Drawing.Point(224, 97);
			button3.Name = "button3";
			button3.Size = new System.Drawing.Size(97, 45);
			button3.TabIndex = 3;
			button3.Text = "shutdown";
			button3.UseVisualStyleBackColor = true;
			button3.Click += button3_Click;
			// 
			// button4
			// 
			button4.Location = new System.Drawing.Point(22, 198);
			button4.Name = "button4";
			button4.Size = new System.Drawing.Size(121, 54);
			button4.TabIndex = 4;
			button4.Text = "cancelProcess";
			button4.UseVisualStyleBackColor = true;
			button4.Click += button4_Click;
			// 
			// button5
			// 
			button5.Location = new System.Drawing.Point(224, 203);
			button5.Name = "button5";
			button5.Size = new System.Drawing.Size(113, 45);
			button5.TabIndex = 5;
			button5.Text = "renameFile";
			button5.UseVisualStyleBackColor = true;
			button5.Click += button5_Click;
			// 
			// button6
			// 
			button6.Location = new System.Drawing.Point(31, 281);
			button6.Name = "button6";
			button6.Size = new System.Drawing.Size(75, 23);
			button6.TabIndex = 6;
			button6.Text = "button6";
			button6.UseVisualStyleBackColor = true;
			button6.Click += button6_Click;
			// 
			// smoothProgressBar1
			// 
			smoothProgressBar1.Location = new System.Drawing.Point(22, 414);
			smoothProgressBar1.Maximum = 100;
			smoothProgressBar1.Minimum = 0;
			smoothProgressBar1.Name = "smoothProgressBar1";
			smoothProgressBar1.ProgressBarColor = System.Drawing.Color.FromArgb(153, 255, 153);
			smoothProgressBar1.Size = new System.Drawing.Size(181, 30);
			smoothProgressBar1.TabIndex = 7;
			smoothProgressBar1.Value = 0;
			// 
			// timer1
			// 
			timer1.Tick += timer1_Tick;
			// 
			// button7
			// 
			button7.Location = new System.Drawing.Point(208, 281);
			button7.Name = "button7";
			button7.Size = new System.Drawing.Size(113, 23);
			button7.TabIndex = 8;
			button7.Text = "testRestApi";
			button7.UseVisualStyleBackColor = true;
			button7.Click += button7_Click;
			// 
			// button8
			// 
			button8.Location = new System.Drawing.Point(31, 328);
			button8.Name = "button8";
			button8.Size = new System.Drawing.Size(75, 23);
			button8.TabIndex = 9;
			button8.Text = "button8";
			button8.UseVisualStyleBackColor = true;
			button8.Click += button8_Click;
			// 
			// richTextBox1
			// 
			richTextBox1.Location = new System.Drawing.Point(302, 384);
			richTextBox1.Name = "richTextBox1";
			richTextBox1.Size = new System.Drawing.Size(100, 96);
			richTextBox1.TabIndex = 10;
			richTextBox1.Text = "";
			// 
			// FormTestAutoCursor
			// 
			AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
			AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			CancelButton = btnMoveCursor;
			ClientSize = new System.Drawing.Size(465, 539);
			Controls.Add(richTextBox1);
			Controls.Add(button8);
			Controls.Add(button7);
			Controls.Add(smoothProgressBar1);
			Controls.Add(button6);
			Controls.Add(button5);
			Controls.Add(button4);
			Controls.Add(button3);
			Controls.Add(button2);
			Controls.Add(button1);
			Controls.Add(btnMoveCursor);
			Name = "FormTestAutoCursor";
			Text = "FormTestAutoCursor";
			ResumeLayout(false);
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
		private System.Windows.Forms.Button button7;
		private System.Windows.Forms.Button button8;
		private System.Windows.Forms.RichTextBox richTextBox1;
	}
}

