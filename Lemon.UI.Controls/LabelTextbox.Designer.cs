using System.Windows.Forms;

namespace Lemon.UI.Controls
{
	partial class LabelTextbox
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
			layout = new System.Windows.Forms.TableLayoutPanel();
			leftLabel = new System.Windows.Forms.Label();
			textBox = new System.Windows.Forms.TextBox();
			rightLabel = new System.Windows.Forms.Label();
			layout.SuspendLayout();
			SuspendLayout();
			// 
			// layout
			// 
			layout.ColumnCount = 3;
			layout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 1F));
			layout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 98F));
			layout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 1F));
			layout.Controls.Add(leftLabel, 0, 0);
			layout.Controls.Add(textBox, 1, 0);
			layout.Controls.Add(rightLabel, 2, 0);
			layout.Dock = System.Windows.Forms.DockStyle.Fill;
			layout.Location = new System.Drawing.Point(0, 0);
			layout.Name = "layout";
			layout.RowCount = 1;
			layout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			layout.Size = new System.Drawing.Size(78, 28);
			layout.TabIndex = 0;
			// 
			// leftLabel
			// 
			leftLabel.AutoSize = true;
			leftLabel.Dock = System.Windows.Forms.DockStyle.Fill;
			leftLabel.Anchor = AnchorStyles.None;
			leftLabel.Location = new System.Drawing.Point(0, 0);
			leftLabel.Margin = new System.Windows.Forms.Padding(0);
			leftLabel.Name = "leftLabel";
			leftLabel.Size = new System.Drawing.Size(1, 28);
			leftLabel.TabIndex = 0;
			leftLabel.Visible = false;
			// 
			// textBox
			// 
			textBox.Dock = System.Windows.Forms.DockStyle.Fill;
			textBox.Location = new System.Drawing.Point(0, 0);
			textBox.Margin = new System.Windows.Forms.Padding(0);
			textBox.Multiline = true;
			textBox.Name = "textBox";
			textBox.Size = new System.Drawing.Size(76, 28);
			textBox.TabIndex = 2;
			// 
			// rightLabel
			// 
			rightLabel.AutoSize = true;
			rightLabel.Dock = System.Windows.Forms.DockStyle.Fill;
			rightLabel.Anchor = AnchorStyles.None;
			rightLabel.Location = new System.Drawing.Point(76, 0);
			rightLabel.Margin = new System.Windows.Forms.Padding(0);
			rightLabel.Name = "rightLabel";
			rightLabel.Size = new System.Drawing.Size(2, 28);
			rightLabel.TabIndex = 1;
			rightLabel.Visible = false;
			// 
			// LabelTextbox
			// 
			AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
			AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			Controls.Add(layout);
			Name = "LabelTextbox";
			Size = new System.Drawing.Size(78, 28);
			layout.ResumeLayout(false);
			layout.PerformLayout();
			ResumeLayout(false);
		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel layout;
		private System.Windows.Forms.Label leftLabel;
		private System.Windows.Forms.Label rightLabel;
		private System.Windows.Forms.TextBox textBox;
	}
}
