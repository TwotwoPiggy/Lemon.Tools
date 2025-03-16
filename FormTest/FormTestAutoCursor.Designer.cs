
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
			pictureBox1 = new System.Windows.Forms.PictureBox();
			listBox1 = new System.Windows.Forms.ListBox();
			switchButton1 = new Lemon.UI.Controls.SwitchButton();
			((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
			SuspendLayout();
			// 
			// btnMoveCursor
			// 
			btnMoveCursor.Location = new System.Drawing.Point(13, 23);
			btnMoveCursor.Margin = new System.Windows.Forms.Padding(4);
			btnMoveCursor.Name = "btnMoveCursor";
			btnMoveCursor.Size = new System.Drawing.Size(189, 41);
			btnMoveCursor.TabIndex = 0;
			btnMoveCursor.Text = "MoveCursor";
			btnMoveCursor.UseVisualStyleBackColor = true;
			btnMoveCursor.Click += btnMoveCursor_Click;
			// 
			// button1
			// 
			button1.Location = new System.Drawing.Point(210, 23);
			button1.Margin = new System.Windows.Forms.Padding(4);
			button1.Name = "button1";
			button1.Size = new System.Drawing.Size(120, 41);
			button1.TabIndex = 1;
			button1.Text = "Screenshot";
			button1.UseVisualStyleBackColor = true;
			button1.Click += button1_Click;
			// 
			// button2
			// 
			button2.Location = new System.Drawing.Point(351, 23);
			button2.Margin = new System.Windows.Forms.Padding(4);
			button2.Name = "button2";
			button2.Size = new System.Drawing.Size(137, 41);
			button2.TabIndex = 2;
			button2.Text = "gpt";
			button2.UseVisualStyleBackColor = true;
			button2.Click += button2_Click;
			// 
			// button3
			// 
			button3.Location = new System.Drawing.Point(23, 95);
			button3.Margin = new System.Windows.Forms.Padding(4);
			button3.Name = "button3";
			button3.Size = new System.Drawing.Size(125, 53);
			button3.TabIndex = 3;
			button3.Text = "shutdown";
			button3.UseVisualStyleBackColor = true;
			button3.Click += button3_Click;
			// 
			// button4
			// 
			button4.Location = new System.Drawing.Point(174, 95);
			button4.Margin = new System.Windows.Forms.Padding(4);
			button4.Name = "button4";
			button4.Size = new System.Drawing.Size(156, 53);
			button4.TabIndex = 4;
			button4.Text = "cancelProcess";
			button4.UseVisualStyleBackColor = true;
			button4.Click += button4_Click;
			// 
			// button5
			// 
			button5.Location = new System.Drawing.Point(362, 95);
			button5.Margin = new System.Windows.Forms.Padding(4);
			button5.Name = "button5";
			button5.Size = new System.Drawing.Size(145, 53);
			button5.TabIndex = 5;
			button5.Text = "renameFile";
			button5.UseVisualStyleBackColor = true;
			button5.Click += button5_Click;
			// 
			// button6
			// 
			button6.Location = new System.Drawing.Point(23, 185);
			button6.Margin = new System.Windows.Forms.Padding(4);
			button6.Name = "button6";
			button6.Size = new System.Drawing.Size(138, 45);
			button6.TabIndex = 6;
			button6.Text = "testProgressBar";
			button6.UseVisualStyleBackColor = true;
			button6.Click += button6_Click;
			// 
			// smoothProgressBar1
			// 
			smoothProgressBar1.Location = new System.Drawing.Point(13, 250);
			smoothProgressBar1.Margin = new System.Windows.Forms.Padding(4);
			smoothProgressBar1.Maximum = 100;
			smoothProgressBar1.Minimum = 0;
			smoothProgressBar1.Name = "smoothProgressBar1";
			smoothProgressBar1.ProgressBarColor = System.Drawing.Color.FromArgb(153, 255, 153);
			smoothProgressBar1.Size = new System.Drawing.Size(233, 35);
			smoothProgressBar1.TabIndex = 7;
			smoothProgressBar1.Value = 0;
			// 
			// timer1
			// 
			timer1.Tick += timer1_Tick;
			// 
			// button7
			// 
			button7.Location = new System.Drawing.Point(23, 308);
			button7.Name = "button7";
			button7.Size = new System.Drawing.Size(141, 39);
			button7.TabIndex = 8;
			button7.Text = "QRCode";
			button7.UseVisualStyleBackColor = true;
			button7.Click += button7_Click;
			// 
			// pictureBox1
			// 
			pictureBox1.Location = new System.Drawing.Point(23, 368);
			pictureBox1.Name = "pictureBox1";
			pictureBox1.Size = new System.Drawing.Size(215, 215);
			pictureBox1.TabIndex = 9;
			pictureBox1.TabStop = false;
			// 
			// listBox1
			// 
			listBox1.FormattingEnabled = true;
			listBox1.Items.AddRange(new object[] { "1", "2", "2", "3" });
			listBox1.Location = new System.Drawing.Point(308, 381);
			listBox1.Name = "listBox1";
			listBox1.Size = new System.Drawing.Size(150, 104);
			listBox1.TabIndex = 10;
			// 
			// switchButton1
			// 
			switchButton1.BorderWidth = 2;
			switchButton1.CloseColor = System.Drawing.Color.Gray;
			switchButton1.CornerRadius = 5;
			switchButton1.IsChecked = false;
			switchButton1.Location = new System.Drawing.Point(321, 186);
			switchButton1.Name = "switchButton1";
			switchButton1.OpenColor = System.Drawing.Color.Green;
			switchButton1.Size = new System.Drawing.Size(85, 32);
			switchButton1.TabIndex = 11;
			switchButton1.Click += switchButton1_Click;
			// 
			// FormTestAutoCursor
			// 
			AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
			AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			CancelButton = btnMoveCursor;
			ClientSize = new System.Drawing.Size(598, 634);
			Controls.Add(switchButton1);
			Controls.Add(listBox1);
			Controls.Add(pictureBox1);
			Controls.Add(button7);
			Controls.Add(smoothProgressBar1);
			Controls.Add(button6);
			Controls.Add(button5);
			Controls.Add(button4);
			Controls.Add(button3);
			Controls.Add(button2);
			Controls.Add(button1);
			Controls.Add(btnMoveCursor);
			Margin = new System.Windows.Forms.Padding(4);
			Name = "FormTestAutoCursor";
			Text = "FormTestAutoCursor";
			((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
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
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.ListBox listBox1;
		private Lemon.UI.Controls.SwitchButton switchButton1;
	}
}

