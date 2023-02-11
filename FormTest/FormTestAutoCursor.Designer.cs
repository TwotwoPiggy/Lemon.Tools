
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
			this.btnMoveCursor = new System.Windows.Forms.Button();
			this.button1 = new System.Windows.Forms.Button();
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
			// FormTestAutoCursor
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnMoveCursor;
			this.ClientSize = new System.Drawing.Size(420, 339);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.btnMoveCursor);
			this.Name = "FormTestAutoCursor";
			this.Text = "FormTestAutoCursor";
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button btnMoveCursor;
		private System.Windows.Forms.Button button1;
	}
}

