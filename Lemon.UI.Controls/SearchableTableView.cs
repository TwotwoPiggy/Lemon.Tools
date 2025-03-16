using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lemon.UI.Controls
{
	public partial class SearchableTableView : UserControl
	{
		#region fields and properties

		public string QueryString => searchTextbox.Text.TrimStart().TrimEnd().ToLower();

		private readonly string _recordCountFormat = "共 {0} 条记录";
		private readonly string _currentPageFormat = "当前页 {0}/{1}";

		#endregion

		#region Page break
		
		/// <summary>
		/// 总记录数
		/// </summary>
		public int RecordCount { get; set; }
		/// <summary>
		/// 总页数
		/// </summary>
		public int PageCount { get; set; }
		/// <summary>
		/// 页数最大记录
		/// </summary>
		public int PageSize { get; set; }
		/// <summary>
		/// 当前页数
		/// </summary>
		public int CurrentPage { get; set; }
		#endregion

		#region public controllers
		public TextBox SearchBox 
		{
			get
			{
				return searchTextbox;
			}
			set
			{
				searchTextbox = value;
			}
		}


		public Button SearchButton
		{
			get
			{
				return searchButton;
			}
			set
			{
				searchButton = value;
			}
		}
		public BindingSource BindingSource
		{
			get
			{
				return bindingSource;
			}
			set
			{
				bindingSource = value;
			}
		}

		public DataGridView View
		{
			get
			{
				return searchableTable;
			}
			set
			{
				searchableTable = value;
			}
		}

		public LabelTextbox PageSizeTextbox
		{
			get
			{
				return pageSizeTextbox;
			}
			set
			{
				pageSizeTextbox = value;
			}
		}

		public Button HomeButton
		{
			get
			{
				return homeBtn;
			}
			set
			{
				homeBtn = value;
			}
		}

		public Button PrePageButton
		{
			get
			{
				return prePageBtn;
			}
			set
			{
				prePageBtn = value;
			}
		}

		public Button NextPageButton
		{
			get
			{
				return nextPageBtn;
			}
			set
			{
				nextPageBtn = value;
			}
		}

		public Button LastPageButton
		{
			get
			{
				return lastPageBtn;
			}
			set
			{
				lastPageBtn = value;
			}
		}

		public TextBox JumpTextbox
		{
			get
			{
				return jumpTextbox;
			}
			set
			{
				jumpTextbox = value;
			}
		}

		public Button JumpButton
		{
			get
			{
				return jumpBtn;
			}
			set
			{
				jumpBtn = value;
			}
		}
		#endregion

		public SearchableTableView()
		{
			InitializeComponent();
			PageSize = 50;
			pageSizeTextbox.EnableBothLabels = true;
		}

		public void InitPageInfo(DataTable dataTable)
		{
			RecordCount = dataTable.Rows.Count;
			PageCount = Convert.ToInt32(Math.Ceiling((double)RecordCount / PageSize));
			CurrentPage = 1;
			InitLabels();
		}
		
		public void InitLabels()
		{
			recordCountLabel.Text = string.Format(_recordCountFormat, RecordCount.ToString());
			currentPageLabel.Text = string.Format(_currentPageFormat, CurrentPage.ToString(), PageCount.ToString());
		}
	}
}
