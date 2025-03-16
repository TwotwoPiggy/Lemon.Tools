using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Lemon.UI.Controls
{
	partial class SearchableTableView
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
			components = new Container();
			baseLayoutPanel = new TableLayoutPanel();
			searchLayoutPanel = new TableLayoutPanel();
			searchLabel = new Label();
			searchTextbox = new TextBox();
			searchButton = new Button();
			searchableTable = new DataGridView();
			pageLayoutPanel = new TableLayoutPanel();
			pageSizeTextbox = new LabelTextbox();
			homeBtn = new Button();
			recordCountLabel = new Label();
			currentPageLabel = new Label();
			prePageBtn = new Button();
			nextPageBtn = new Button();
			lastPageBtn = new Button();
			jumpBtn = new Button();
			jumpTextbox = new TextBox();
			bindingSource = new BindingSource(components);
			listBox1 = new ListBox();
			baseLayoutPanel.SuspendLayout();
			searchLayoutPanel.SuspendLayout();
			((ISupportInitialize)searchableTable).BeginInit();
			pageLayoutPanel.SuspendLayout();
			((ISupportInitialize)bindingSource).BeginInit();
			SuspendLayout();
			// 
			// baseLayoutPanel
			// 
			baseLayoutPanel.ColumnCount = 1;
			baseLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
			baseLayoutPanel.Controls.Add(searchLayoutPanel, 0, 0);
			baseLayoutPanel.Controls.Add(searchableTable, 0, 1);
			baseLayoutPanel.Controls.Add(pageLayoutPanel, 0, 2);
			baseLayoutPanel.Dock = DockStyle.Fill;
			baseLayoutPanel.Location = new Point(0, 0);
			baseLayoutPanel.Name = "baseLayoutPanel";
			baseLayoutPanel.RowCount = 3;
			baseLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
			baseLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 82F));
			baseLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 8F));
			baseLayoutPanel.Size = new Size(1045, 487);
			baseLayoutPanel.TabIndex = 0;
			// 
			// searchLayoutPanel
			// 
			searchLayoutPanel.ColumnCount = 3;
			searchLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 14.83871F));
			searchLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70.32258F));
			searchLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 14.83871F));
			searchLayoutPanel.Controls.Add(searchLabel, 0, 0);
			searchLayoutPanel.Controls.Add(searchTextbox, 1, 0);
			searchLayoutPanel.Controls.Add(searchButton, 2, 0);
			searchLayoutPanel.Dock = DockStyle.Fill;
			searchLayoutPanel.Location = new Point(0, 0);
			searchLayoutPanel.Margin = new Padding(0);
			searchLayoutPanel.Name = "searchLayoutPanel";
			searchLayoutPanel.RowCount = 1;
			searchLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
			searchLayoutPanel.Size = new Size(1045, 48);
			searchLayoutPanel.TabIndex = 1;
			// 
			// searchLabel
			// 
			searchLabel.AutoSize = true;
			searchLabel.Dock = DockStyle.Fill;
			searchLabel.Font = new Font("Microsoft YaHei UI", 13F, FontStyle.Bold);
			searchLabel.Location = new Point(0, 0);
			searchLabel.Margin = new Padding(0);
			searchLabel.Name = "searchLabel";
			searchLabel.Size = new Size(155, 48);
			searchLabel.TabIndex = 0;
			searchLabel.Text = "检索:";
			searchLabel.TextAlign = ContentAlignment.MiddleCenter;
			// 
			// searchTextbox
			// 
			searchTextbox.Dock = DockStyle.Fill;
			searchTextbox.Location = new Point(156, 1);
			searchTextbox.Margin = new Padding(1);
			searchTextbox.Multiline = true;
			searchTextbox.Name = "searchTextbox";
			searchTextbox.Size = new Size(732, 46);
			searchTextbox.TabIndex = 1;
			// 
			// searchButton
			// 
			searchButton.Dock = DockStyle.Fill;
			searchButton.Font = new Font("Microsoft YaHei UI", 10F, FontStyle.Bold);
			searchButton.Location = new Point(892, 3);
			searchButton.Name = "searchButton";
			searchButton.Size = new Size(150, 42);
			searchButton.TabIndex = 2;
			searchButton.Text = "查询";
			searchButton.UseVisualStyleBackColor = true;
			// 
			// searchableTable
			// 
			searchableTable.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;
			searchableTable.BorderStyle = BorderStyle.Fixed3D;
			searchableTable.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			searchableTable.Dock = DockStyle.Fill;
			searchableTable.EditMode = DataGridViewEditMode.EditOnEnter;
			searchableTable.Location = new Point(3, 51);
			searchableTable.Name = "searchableTable";
			searchableTable.RowHeadersWidth = 51;
			searchableTable.Size = new Size(1039, 393);
			searchableTable.TabIndex = 2;
			// 
			// pageLayoutPanel
			// 
			pageLayoutPanel.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
			pageLayoutPanel.ColumnCount = 12;
			pageLayoutPanel.ColumnStyles.Add(new ColumnStyle());
			pageLayoutPanel.ColumnStyles.Add(new ColumnStyle());
			pageLayoutPanel.ColumnStyles.Add(new ColumnStyle());
			pageLayoutPanel.ColumnStyles.Add(new ColumnStyle());
			pageLayoutPanel.ColumnStyles.Add(new ColumnStyle());
			pageLayoutPanel.ColumnStyles.Add(new ColumnStyle());
			pageLayoutPanel.ColumnStyles.Add(new ColumnStyle());
			pageLayoutPanel.ColumnStyles.Add(new ColumnStyle());
			pageLayoutPanel.ColumnStyles.Add(new ColumnStyle());
			pageLayoutPanel.ColumnStyles.Add(new ColumnStyle());
			pageLayoutPanel.ColumnStyles.Add(new ColumnStyle());
			pageLayoutPanel.ColumnStyles.Add(new ColumnStyle());
			pageLayoutPanel.Controls.Add(pageSizeTextbox, 2, 0);
			pageLayoutPanel.Controls.Add(homeBtn, 4, 0);
			pageLayoutPanel.Controls.Add(recordCountLabel, 0, 0);
			pageLayoutPanel.Controls.Add(currentPageLabel, 1, 0);
			pageLayoutPanel.Controls.Add(prePageBtn, 5, 0);
			pageLayoutPanel.Controls.Add(nextPageBtn, 6, 0);
			pageLayoutPanel.Controls.Add(lastPageBtn, 7, 0);
			pageLayoutPanel.Controls.Add(jumpBtn, 10, 0);
			pageLayoutPanel.Controls.Add(jumpTextbox, 9, 0);
			pageLayoutPanel.Controls.Add(listBox1, 11, 0);
			pageLayoutPanel.Dock = DockStyle.Fill;
			pageLayoutPanel.Location = new Point(0, 447);
			pageLayoutPanel.Margin = new Padding(0);
			pageLayoutPanel.Name = "pageLayoutPanel";
			pageLayoutPanel.RowCount = 1;
			pageLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
			pageLayoutPanel.Size = new Size(1045, 40);
			pageLayoutPanel.TabIndex = 3;
			// 
			// pageSizeTextbox
			// 
			pageSizeTextbox.Anchor = AnchorStyles.None;
			pageSizeTextbox.Content = "";
			pageSizeTextbox.EnableBothLabels = true;
			pageSizeTextbox.EnableLeftLabel = true;
			pageSizeTextbox.EnableRightLabel = true;
			pageSizeTextbox.LeftLabelText = "每页";
			pageSizeTextbox.Location = new Point(187, 5);
			pageSizeTextbox.Name = "pageSizeTextbox";
			pageSizeTextbox.RightLabelText = "行";
			pageSizeTextbox.Size = new Size(196, 30);
			pageSizeTextbox.TabIndex = 2;
			pageSizeTextbox.TextBoxSize = new Size(78, 30);
			// 
			// homeBtn
			// 
			homeBtn.Anchor = AnchorStyles.None;
			homeBtn.Location = new Point(391, 5);
			homeBtn.Name = "homeBtn";
			homeBtn.Size = new Size(47, 30);
			homeBtn.TabIndex = 1;
			homeBtn.Text = "首页";
			homeBtn.UseVisualStyleBackColor = true;
			// 
			// recordCountLabel
			// 
			recordCountLabel.Anchor = AnchorStyles.None;
			recordCountLabel.AutoSize = true;
			recordCountLabel.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Bold);
			recordCountLabel.Location = new Point(4, 10);
			recordCountLabel.Name = "recordCountLabel";
			recordCountLabel.Size = new Size(86, 19);
			recordCountLabel.TabIndex = 0;
			recordCountLabel.Text = "共 0 条记录";
			// 
			// currentPageLabel
			// 
			currentPageLabel.Anchor = AnchorStyles.None;
			currentPageLabel.AutoSize = true;
			currentPageLabel.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Bold);
			currentPageLabel.Location = new Point(97, 10);
			currentPageLabel.Name = "currentPageLabel";
			currentPageLabel.Size = new Size(83, 19);
			currentPageLabel.TabIndex = 3;
			currentPageLabel.Text = "当前页 0/0";
			// 
			// prePageBtn
			// 
			prePageBtn.Anchor = AnchorStyles.None;
			prePageBtn.Location = new Point(445, 5);
			prePageBtn.Name = "prePageBtn";
			prePageBtn.Size = new Size(62, 30);
			prePageBtn.TabIndex = 4;
			prePageBtn.Text = "上一页";
			prePageBtn.UseVisualStyleBackColor = true;
			// 
			// nextPageBtn
			// 
			nextPageBtn.Anchor = AnchorStyles.None;
			nextPageBtn.Location = new Point(514, 5);
			nextPageBtn.Name = "nextPageBtn";
			nextPageBtn.Size = new Size(62, 30);
			nextPageBtn.TabIndex = 5;
			nextPageBtn.Text = "下一页";
			nextPageBtn.UseVisualStyleBackColor = true;
			// 
			// lastPageBtn
			// 
			lastPageBtn.Anchor = AnchorStyles.None;
			lastPageBtn.Location = new Point(583, 5);
			lastPageBtn.Name = "lastPageBtn";
			lastPageBtn.Size = new Size(62, 30);
			lastPageBtn.TabIndex = 6;
			lastPageBtn.Text = "末页";
			lastPageBtn.UseVisualStyleBackColor = true;
			// 
			// jumpBtn
			// 
			jumpBtn.Anchor = AnchorStyles.None;
			jumpBtn.Location = new Point(785, 5);
			jumpBtn.Name = "jumpBtn";
			jumpBtn.Size = new Size(94, 30);
			jumpBtn.TabIndex = 7;
			jumpBtn.Text = "跳转";
			jumpBtn.UseVisualStyleBackColor = true;
			// 
			// jumpTextbox
			// 
			jumpTextbox.Anchor = AnchorStyles.None;
			jumpTextbox.Location = new Point(653, 5);
			jumpTextbox.Multiline = true;
			jumpTextbox.Name = "jumpTextbox";
			jumpTextbox.Size = new Size(125, 30);
			jumpTextbox.TabIndex = 8;
			// 
			// listBox1
			// 
			listBox1.FormattingEnabled = true;
			listBox1.Items.AddRange(new object[] { "1", "2", "3", "4", "5" });
			listBox1.Location = new Point(886, 4);
			listBox1.Name = "listBox1";
			listBox1.Size = new Size(59, 24);
			listBox1.TabIndex = 9;
			// 
			// SearchableTableView
			// 
			AutoScaleDimensions = new SizeF(9F, 20F);
			AutoScaleMode = AutoScaleMode.Font;
			Controls.Add(baseLayoutPanel);
			Name = "SearchableTableView";
			Size = new Size(1045, 487);
			baseLayoutPanel.ResumeLayout(false);
			searchLayoutPanel.ResumeLayout(false);
			searchLayoutPanel.PerformLayout();
			((ISupportInitialize)searchableTable).EndInit();
			pageLayoutPanel.ResumeLayout(false);
			pageLayoutPanel.PerformLayout();
			((ISupportInitialize)bindingSource).EndInit();
			ResumeLayout(false);
		}

		#endregion


		private TableLayoutPanel baseLayoutPanel;
		private TableLayoutPanel searchLayoutPanel;
		private Label searchLabel;
		private TextBox searchTextbox;
		private Button searchButton;
		private BindingSource bindingSource;
		private DataGridView searchableTable;
		private TableLayoutPanel pageLayoutPanel;
		private Label recordCountLabel;
		private Button homeBtn;
		private LabelTextbox pageSizeTextbox;
		private Label currentPageLabel;
		private Button prePageBtn;
		private Button nextPageBtn;
		private Button lastPageBtn;
		private Button jumpBtn;
		private TextBox jumpTextbox;
		private ListBox listBox1;
	}
}
