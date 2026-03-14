
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
            btnMoveCursor = new Button();
            button1 = new Button();
            button2 = new Button();
            button3 = new Button();
            button4 = new Button();
            button5 = new Button();
            button6 = new Button();
            smoothProgressBar1 = new Lemon.UI.Controls.SmoothProgressBar();
            timer1 = new System.Windows.Forms.Timer(components);
            button7 = new Button();
            pictureBox1 = new PictureBox();
            listBox1 = new ListBox();
            switchButton1 = new Lemon.UI.Controls.SwitchButton();
            btnSelectFile = new Button();
            textBox1 = new TextBox();
            btnProcessImage = new Button();
            button8 = new Button();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // btnMoveCursor
            // 
            btnMoveCursor.Location = new Point(10, 20);
            btnMoveCursor.Name = "btnMoveCursor";
            btnMoveCursor.Size = new Size(147, 35);
            btnMoveCursor.TabIndex = 0;
            btnMoveCursor.Text = "MoveCursor";
            btnMoveCursor.UseVisualStyleBackColor = true;
            btnMoveCursor.Click += btnMoveCursor_Click;
            // 
            // button1
            // 
            button1.Location = new Point(163, 20);
            button1.Name = "button1";
            button1.Size = new Size(93, 35);
            button1.TabIndex = 1;
            button1.Text = "Screenshot";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.Location = new Point(273, 20);
            button2.Name = "button2";
            button2.Size = new Size(107, 35);
            button2.TabIndex = 2;
            button2.Text = "gpt";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // button3
            // 
            button3.Location = new Point(18, 81);
            button3.Name = "button3";
            button3.Size = new Size(97, 45);
            button3.TabIndex = 3;
            button3.Text = "shutdown";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // button4
            // 
            button4.Location = new Point(135, 81);
            button4.Name = "button4";
            button4.Size = new Size(121, 45);
            button4.TabIndex = 4;
            button4.Text = "cancelProcess";
            button4.UseVisualStyleBackColor = true;
            button4.Click += button4_Click;
            // 
            // button5
            // 
            button5.Location = new Point(282, 81);
            button5.Name = "button5";
            button5.Size = new Size(113, 45);
            button5.TabIndex = 5;
            button5.Text = "renameFile";
            button5.UseVisualStyleBackColor = true;
            button5.Click += button5_Click;
            // 
            // button6
            // 
            button6.Location = new Point(18, 157);
            button6.Name = "button6";
            button6.Size = new Size(107, 38);
            button6.TabIndex = 6;
            button6.Text = "testProgressBar";
            button6.UseVisualStyleBackColor = true;
            button6.Click += button6_Click;
            // 
            // smoothProgressBar1
            // 
            smoothProgressBar1.Location = new Point(10, 212);
            smoothProgressBar1.Name = "smoothProgressBar1";
            smoothProgressBar1.Size = new Size(181, 30);
            smoothProgressBar1.TabIndex = 7;
            // 
            // timer1
            // 
            timer1.Tick += timer1_Tick;
            // 
            // button7
            // 
            button7.Location = new Point(18, 262);
            button7.Margin = new Padding(2, 3, 2, 3);
            button7.Name = "button7";
            button7.Size = new Size(110, 33);
            button7.TabIndex = 8;
            button7.Text = "QRCode";
            button7.UseVisualStyleBackColor = true;
            button7.Click += button7_Click;
            // 
            // pictureBox1
            // 
            pictureBox1.Location = new Point(18, 313);
            pictureBox1.Margin = new Padding(2, 3, 2, 3);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(167, 183);
            pictureBox1.TabIndex = 9;
            pictureBox1.TabStop = false;
            // 
            // listBox1
            // 
            listBox1.FormattingEnabled = true;
            listBox1.Items.AddRange(new object[] { "1", "2", "2", "3" });
            listBox1.Location = new Point(240, 324);
            listBox1.Margin = new Padding(2, 3, 2, 3);
            listBox1.Name = "listBox1";
            listBox1.Size = new Size(118, 89);
            listBox1.TabIndex = 10;
            // 
            // switchButton1
            // 
            switchButton1.Location = new Point(250, 158);
            switchButton1.Margin = new Padding(2, 3, 2, 3);
            switchButton1.Name = "switchButton1";
            switchButton1.Size = new Size(66, 27);
            switchButton1.TabIndex = 11;
            switchButton1.Click += switchButton1_Click;
            // 
            // btnSelectFile
            // 
            btnSelectFile.Location = new Point(470, 17);
            btnSelectFile.Name = "btnSelectFile";
            btnSelectFile.Size = new Size(86, 41);
            btnSelectFile.TabIndex = 12;
            btnSelectFile.Text = "Select File";
            btnSelectFile.UseVisualStyleBackColor = true;
            btnSelectFile.Click += btnSelectFile_Click;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(562, 26);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(342, 23);
            textBox1.TabIndex = 13;
            // 
            // btnProcessImage
            // 
            btnProcessImage.Location = new Point(451, 75);
            btnProcessImage.Name = "btnProcessImage";
            btnProcessImage.Size = new Size(132, 57);
            btnProcessImage.TabIndex = 14;
            btnProcessImage.Text = "Process Image";
            btnProcessImage.UseVisualStyleBackColor = true;
            btnProcessImage.Click += btnProcessImage_Click;
            // 
            // button8
            // 
            button8.Location = new Point(432, 156);
            button8.Name = "button8";
            button8.Size = new Size(181, 23);
            button8.TabIndex = 15;
            button8.Text = "Test TimedMessageBox";
            button8.UseVisualStyleBackColor = true;
            button8.Click += button8_Click;
            // 
            // FormTestAutoCursor
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnMoveCursor;
            ClientSize = new Size(1038, 539);
            Controls.Add(button8);
            Controls.Add(btnProcessImage);
            Controls.Add(textBox1);
            Controls.Add(btnSelectFile);
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
            Name = "FormTestAutoCursor";
            Text = "FormTestAutoCursor";
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
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
        private Button btnSelectFile;
        private TextBox textBox1;
        private Button btnProcessImage;
        private Button button8;
    }
}

