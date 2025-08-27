using CommonTools;
using HtmlAgilityPack;
using HttpManager;
using Newtonsoft.Json;
using OcrApi;
using OcrApi.Models;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using SQLite;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace Test
{
	class Program
	{
		public static void Main(string[] args)
		{
			var result = GetCmbcAccumulatedGoldPrice().ConfigureAwait(false).GetAwaiter().GetResult();
            Console.WriteLine(result);
			//var result = SystemManager.GetServiceValue("i8042prt");
			//if (result.Contains("4  RUNNING"))
			//{
			//             Console.WriteLine("Running");
			//}
			//         else
			//         {
			//	Console.WriteLine("STOPPED");
			//}
			//         Console.WriteLine(result);

			//ConnectWifi();
		}

		public static async Task<decimal> GetCmbcAccumulatedGoldPrice()
		{
			using (var client = new HttpClient())
			{
				// 民生银行积存金页面（示例URL，需确认实际地址）
				//var html = await client.GetAsync("https://www.cngold.org/img_date/bank_gold.html");
				var page = ScrapeLazyLoadedData();//await html.Content.ReadAsStringAsync();
				// 使用HtmlAgilityPack解析
				var doc = new HtmlDocument();
				doc.LoadHtml(page);

				// 通过XPath定位价格（需根据实际网页结构调整）
				var node = doc.DocumentNode.SelectSingleNode("//td[@class='JO_283982q63']");
				return decimal.Parse(node.ChildNodes.First().InnerText.Trim());
			}
		}

		public static string ScrapeLazyLoadedData()
		{
			var options = new ChromeOptions();
			options.AddArgument("--headless"); // 无头模式
			using (var driver = new ChromeDriver(options))
			{
				driver.Navigate().GoToUrl("https://www.cngold.org/img_date/bank_gold.html");

				// 模拟滚动到底部 5 次
				for (int i = 0; i < 5; i++)
				{
					((IJavaScriptExecutor)driver).ExecuteScript(
						"window.scrollTo(0, document.body.scrollHeight);");
					Thread.Sleep(2000); // 等待数据加载
				}

				// 获取完整页面源码
				return driver.PageSource;
				// 使用 HtmlAgilityPack 解析数据...
			}
		}

		public static void ConnectWifi()
		{
			var result = WifiManager.IsConnectingAsync("Lemony_5G").ConfigureAwait(false).GetAwaiter().GetResult();
			Console.WriteLine(result);
			//WifiManager.ConnectWifiAsync("Lemon").ConfigureAwait(false).GetAwaiter().GetResult();
		}

		public static void GetWifi()
		{
			//foreach (var item in WifiManager.GetWifiListAsync().ConfigureAwait(false).GetAwaiter().GetResult())
            Console.WriteLine(WifiManager.GetConnectedWifiAysnc().ConfigureAwait(false).GetAwaiter().GetResult());
            
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
					//var request = new FormHttpRequest(httpClient);
					//request.PostAsync();
					Console.WriteLine($"{url} result is {response.StatusCode}");
				}
				catch (Exception)
				{
				}
			}
			var content = "15:50目“孕G\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n乡\n〈〈菱完′…】zb尿一\n鲜朗旗舰店\n鲜朗低温烘焙猫粮冻干生骨…到手#8.63\n才|数量x1,烘焙猎粑禽内试吃装50g*#14.8\n3袋\n\n\n\n\n\n「退敦/售后)(加购物车\n\n\n\n\n\n\n\n\n\n\n\n\n\n实付款合计#8.63〉\n订单编号2970539989751复制\n支付方式银行卡支付\n发祥类型不开发票\n支付时间2024-09-2300:04:12\n下单时间2024-09-2300:04:02\n陆送方式邹政电商标快\n收货信息陈二二150****2462\n\n\n\n\n\n\n\n\n\n\n\n\n\n收货地址“上海宝山区罗泾镇上海市上海市宝山区罗\n泾镇潘新路255弄204号1202室200949\n\n收起~\n\n快速解决问题\n\n商品降价怎么办怎么申请售后更多\n\n\n\n更多,查看物流.,退款/售后,\n\n\n\n\n";
			var pattern = @"\n(.*店)\n[\s\S]*数量x([0-9]{1,3}).*,(.*g)[\s\S]*实付款合计#([0-9]+\.[0-9]+)[\s\S]*订单编号([0-9]{12})[\s\S]*下单时间([0-9]{4}-[0-9]{2}-[0-9]{2})";
			var reg = new Regex(pattern, RegexOptions.IgnoreCase);
			var groups = reg.Match(content).Groups;
			var id = groups[5].Value;
			var name = groups[3].Value.Replace("猎粑","猫粮").Replace("内","肉");
			var shopName = groups[1].Value;
			var count = groups[2].Value;
			var price2 = groups[4].Value;
			var date = groups[6].Value;

			Console.WriteLine($"{shopName}的{name} {count}个,{price2}元 订单编号{id} 日期{date}");

		}
		public enum PlatformType
		{
			JD = 0,
			Taobao = 1,
			PDD = 2,
			Douyin = 3
		}

		public static void TestSystemManager()
		{
			
			//SystemManager.SetServiceValue("i8042prt", "start=disabled");
			//SystemManager.SetServiceValue("i8042prt", "start=auto");
		}
		public static void TestOCRHelper()
		{
			var picPath = @"V:\Screenshots\IMG_5816.PNG";
			//var picPath = @"D:\Computer\Projects\Samples\OCR-tesseract\tesseract-samples\src\Tesseract.ConsoleDemo\IMG_5783.PNG";
			var tessdata = @"D:\Computer\Projects\Lemon.Tools\OcrApi\tessdata";
			var ocrHelper = new OCRHelper(tessdata);
			picPath = ocrHelper.ReduceImageNoise(picPath);
			var content = ocrHelper.GetTextFromPicture(picPath, Languages.Chinese_Simplified, Tesseract.EngineMode.TesseractAndLstm).Replace(" ", string.Empty);

			Console.WriteLine(content);
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
		//public static async void GetPictures(IEnumerable<string> urls)
		//{
		//	var httpClientFactory = ServiceHelper.GetHttpClientFactory();
		//	var results = new List<string>();
		//	foreach (var url in urls)
		//	{
		//		var urlToGet = HttpUtility.HtmlEncode($"http://zlzf.fgj.shmh.gov.cn/MhgzfWeb/File/{url}宝铭苑{url}.jpg");
		//		try
		//		{

		//			//var response = await httpClientFactory.GetAsync(urlToGet);
		//			//var request = new FormHttpRequest(httpClientFactory);
		//			//request.PostAsync();
		//			//Console.WriteLine($"{url} result is {response.StatusCode}");
		//		}
		//		catch (Exception)
		//		{

		//			throw;
		//		}

		//	}
		//}

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

	public class Student : IEnumerable
	{
		[PrimaryKey,Column("id")]
        public long Id { get; set; }
		[Column("name")]
        public string Name { get; set; }
        public int Test { get; set; }
        public int SchoolId { get; set; }

		public IEnumerator GetEnumerator()
		{
			throw new NotImplementedException();
		}
	}

	public class School
	{
		[PrimaryKey, Column("id")]
		public long Id { get; set; }
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
