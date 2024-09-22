using CommonTools;
using HttpManager;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace Test
{
    class Program
	{
		public static void Main(string[] args)
		{
			TestDb();
		}


		public static void TestDb()
		{
			var sqlHelper = new SQLiteHelper();
			sqlHelper.SetConnectionString(@".\default.db");
			sqlHelper.Db.CreateTable<Student>();
			var result = sqlHelper.Db.Table<Student>().FirstOrDefault(student=>student.Name == "Mike");
            Console.WriteLine($"Student {result.Name}'s Id is {result.Id}.");
		}

		#region old tests

		public static void TestLazy()
		{
			var lazyTest = new Lazy<Test>();
			var test = lazyTest.Value;
			test.Action();
		}

		public static void TestLazyInstance()
		{
			var t = Test.Instance;
			t.Action();
		}
		public static async void GetPictures(IEnumerable<string> urls)
		{
			var httpClient = new HttpClient();
			var results = new List<string>();
			foreach (var url in urls)
			{
				var urlToGet = HttpUtility.HtmlEncode($"http://zlzf.fgj.shmh.gov.cn/MhgzfWeb/File/{url}宝铭苑{url}.jpg");
				try
				{

					var response = await httpClient.GetAsync(urlToGet);
					var request = new FormHttpRequest(httpClient);
					//request.PostAsync();
					Console.WriteLine($"{url} result is {response.StatusCode}");
				}
				catch (Exception)
				{

					throw;
				}

			}
		}

		public static void Move()
		{
			var s = @"\\SEAGATE-D2\OneTwoNas\Films\Test\Test\test.txt";
			var n = @"\\SEAGATE-D2\OneTwoNas\Films\Test\Test\test2.txt";
			FileManager.RenameFile(s, n);
		}

		public static string GetName()
		{
			var s = @"\\SEAGATE-D2\OneTwoNas\Films\新闻女王";
			var f = new DirectoryInfo(s);
			//f.Attributes = FileAttributes.A
			return f.Attributes.ToString();
		}

		public static string GetNewName()
		{
			string s = "The.QueenEP10.of.NEWS.2023..EP01.HD1080P.X264.AAC.Cantonese.CHS.BDYS";
			Regex re = new Regex(@"\.(EP[0-9]{2})\.");
			var mc = re.Match(s).Groups.Values.LastOrDefault();
			return mc.Value;
		}

		public static string GetNewName1()
		{
			string s = "格林.GrimmS01E19.S01E18.Chi_Eng.HR-HDTV.AC3.1024X576.x264-YYeTs人人影视.mkv";
			Regex re = new Regex(@"\.(S[0-9]{2}E[0-9]{2})\.");
			var mc = re.Match(s).Groups.Values.LastOrDefault();
			return mc.Value;
		}
		#endregion



	}

	public class Student
	{
		[PrimaryKey,Column("id")]
        public long Id { get; set; }
		[Column("name")]
        public string Name { get; set; }
    }

	public class Test
	{
		private static readonly Lazy<Test> _instance = new(() => new());
		public static Test Instance => _instance.Value;

		private Test()
        {
            Console.WriteLine("this is constructor");
        }

		public void Action()
		{
            Console.WriteLine("this is the action method");
		}
    }
}
