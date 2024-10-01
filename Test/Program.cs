﻿using CommonTools;
using HttpManager;
using OcrApi;
using OcrApi.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Test
{
	class Program
	{
		public static void Main(string[] args)
		{
			var test = "\"15:50 目 “ 孕 G\\n\\n \\n\\n \\n\\n \\n\\n \\n\\n \\n\\n \\n\\n \\n\\n \\n\\n \\n\\n \\n\\n \\n\\n乡\\n〈 〈菱 完′…】z b 尿 一\\n鲜 朗 旗 舰 店\\n鲜 朗 低 温 烘 焙 猫 粮 冻 干 生 骨 … 到 手 #8.63\\n才 | 数 量 x1, 烘 焙 猎 粑 禽 内 试 吃 装 50g* #14.8\\n3 袋\\n\\n \\n\\n \\n\\n「 退 敦 / 售 后 ) ( 加 购 物 车\\n\\n \\n\\n \\n\\n \\n\\n \\n\\n \\n\\n \\n\\n实 付 款 合 计 #8.63 〉\\n订 单 编 号 2970539989751 复 制\\n支 付 方 式 银 行 卡 支 付\\n发 祥 类 型 不 开 发 票\\n支 付 时 间 2024-09-23 00:04:12\\n下 单 时 间 2024-09-23 00:04:02\\n陆 送 方 式 邹 政 电 商 标 快\\n收 货 信 息 陈 二 二 150****2462\\n\\n \\n\\n \\n\\n \\n\\n \\n\\n \\n\\n \\n\\n收 货 地 址 “ 上 海 宝 山 区 罗 泾 镇 上 海 市 上 海 市 宝 山 区 罗\\n泾 镇 潘 新 路 255 弄 204 号 1202 室 200949\\n\\n收 起 ~\\n\\n快 速 解 决 问 题\\n\\n商 品 降 价 怎 么 办 怎 么 申 请 售 后 更 多\\n\\n \\n\\n更 多 , 查 看 物 流 ., 退 款 / 售 后 ,\\n\\n \\n\\n \\n\"";
			test = test.Replace(" ", string.Empty);
            Console.WriteLine(PlatformType.JD.ToString());
			//TestDb();
			//TestOCRHelper();
		}


		public enum PlatformType
		{
			JD = 0,
			Taobao = 1,
			PDD = 2,
			Douyin = 3
		}
		public static void TestOCRHelper()
		{
			var picPath = @"V:\Screenshots\IMG_5816.PNG";
			//var picPath = @"D:\Computer\Projects\Samples\OCR-tesseract\tesseract-samples\src\Tesseract.ConsoleDemo\IMG_5783.PNG";
			var tessdata = @"D:\Computer\Projects\Lemon.Tools\OcrApi\tessdata";
			var ocrHelper = new OCRHelper(tessdata);
			picPath = ocrHelper.ReduceImageNoise(picPath);
            Console.WriteLine(ocrHelper.GetTextFromPicture(picPath, Languages.Chinese_Simplified, Tesseract.EngineMode.TesseractAndLstm));
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

	public class Student
	{
		[PrimaryKey,Column("id")]
        public long Id { get; set; }
		[Column("name")]
        public string Name { get; set; }
        public int Test { get; set; }
        public int SchoolId { get; set; }
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
