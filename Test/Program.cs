using CommonTools;
using HttpManager;
using SQLite;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Test
{
	class Program
	{
		public static void Main(string[] args)
		{
			TestHttpClientHelper();
		}

		public static void TestHttpClientHelper()
		{
			//GET https://jsonplaceholder.typicode.com/todos?userId=1&completed=false
			var httpClientFactory = ServiceHelper.GetHttpClientFactory();
			var httpHelper = new HttpClientHelper(httpClientFactory);
			httpHelper.SetBaseAddress("https://jsonplaceholder.typicode.com/todos");
			var @params = new Dictionary<string, object> 
			{
				{ "userId" , 1 },
				{ "completed" , false},
			};
			httpHelper.AddParameters(@params);
			var response = httpHelper.GetAsync(string.Empty).Result;
			Console.WriteLine(response);
		}

		public static void GetDictionary()
		{
			IDictionary<string, object> list = new Dictionary<string, object>
			{
				{ "key2", "new value2" },
				{ "key4", "value4" }
				//new KeyValuePair<string, string>(null, "value2"),
				//new KeyValuePair<string, string>("", "value3"),
				//new KeyValuePair<string, string>(string.Empty, "value4")
			};

			var parameters = new Dictionary<string, string>
			{
				{ "key1", "value1" },
				{ "key2", "value2" },
				{ "key3", "value3" }
			};
			//parameters = parameters.Concat(list).ToDictionary(kv => kv.Key, kv => kv.Value);
			//parameters = LinqExtensions.MergeDictionaries(parameters, list);
			parameters.ConcatDictionary(list);
			foreach (var parameter in parameters)
			{
				Console.WriteLine($"key:{parameter.Key}, value:{parameter.Value}");
			}
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
			var httpClientFactory = ServiceHelper.GetHttpClientFactory();
			var results = new List<string>();
			foreach (var url in urls)
			{
				var urlToGet = HttpUtility.HtmlEncode($"http://zlzf.fgj.shmh.gov.cn/MhgzfWeb/File/{url}宝铭苑{url}.jpg");
				try
				{

					//var response = await httpClientFactory.GetAsync(urlToGet);
					//var request = new FormHttpRequest(httpClientFactory);
					//request.PostAsync();
					//Console.WriteLine($"{url} result is {response.StatusCode}");
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
