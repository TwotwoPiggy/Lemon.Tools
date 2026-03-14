using CatFoodManager.Core.Models;
using CommonTools;
using Google.GenAI;
using Google.GenAI.Types;
using Microsoft.Data.SqlClient;
using Microsoft.Graphics.Imaging;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Shapes;
using Microsoft.Windows.AI;
using Microsoft.Windows.AI.Imaging;
using OcrApi;
using OcrApi.Models;
using PaddleOCRSharp;
using SQLite;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Streams;

namespace Test
{
	public class BaseClass
	{
		public virtual void Show()
		{
            Console.WriteLine("this is base class");
		}

    }

    public class ExtendClass: BaseClass
    {
        public new void Show()
        {
            Console.WriteLine("this is extend class");
        }
    }

    class Program
	{
		public static async Task Main(string[] args)
		{
			await AIPoc.RunTest();
			// run the GeminiOcrService integration test
			//await RunGeminiOcrServiceTest();
            //var baseClass = new BaseClass();
            //baseClass.Show();
            //var extendClass = new ExtendClass();
            //extendClass.Show();
            //         BaseClass mixedClass = new ExtendClass();
            //mixedClass.Show();


            //string data = "2025C#高性能指南";
            //var numberSpan = data.AsSpan(0,4);
            //         //Console.WriteLine(typeof(numberSpan));
            //Console.WriteLine(numberSpan);


            //var result = GetCmbcAccumulatedGoldPrice().ConfigureAwait(false).GetAwaiter().GetResult();
            //TestNCMConverter();

            //Console.WriteLine(result);
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

            //var filePath  = @"C:\Users\Lemony\Desktop\test.jpg";
            //var imageBuffer =LoadImageBufferFromFileAsync(filePath).ConfigureAwait(false).GetAwaiter().GetResult();
            //var text = RecognizeTextFromSoftwareBitmap(imageBuffer).ConfigureAwait(false).GetAwaiter().GetResult();
            //Console.WriteLine(text);
        }

        #region OCR
        public static async Task GenerateImageCaptionAsync()
        {
            // --- 在调用任何 Gemini 代码之前执行 ---
            // 1. 定义代理服务器
            var proxy = new WebProxy
            {
                Address = new Uri("http://127.0.0.1:10808"),
                BypassProxyOnLocal = true
            };
            HttpClient.DefaultProxy = proxy;


            // 4. 在初始化 GenAI Client 时指定 HttpClient
            var client = new Client(apiKey: "");

            byte[] imageBytes = System.IO.File.ReadAllBytes(@"C:\Users\Lemony\Desktop\testfood.jpg");
            var imagePart = Part.FromBytes(imageBytes, mimeType: "image/jpeg");

            // 将图片和文本 prompt 放在同一个 Content 对象中
            var prompt = new StringBuilder();
            prompt.AppendLine("你是一个OCR助手, 现在需要解析这张截图, 并获取里面的购物信息, 具体需要获取的内容包含:");
            prompt.AppendLine("1. 商品名称, 格式: 包含品牌名称+产品名称+规格，品牌名称和产品名称之间用空格分隔，产品名称和规格之间用空格分隔，规格要包含单位（g、kg、ml、L等）。如果有多个规格，使用*分隔，例如100g * 1，如果没有规格，则只包含品牌名称和产品名称，例如魔宝 蓝莓兔脆脆乐主食冻干 100g * 1。但是不能太长, 精选其中的主要内容, 如果有名字, 类似蓝莓兔 脆脆乐这种特殊名词可以保留, 而全价, 猫咪, 高蛋白, 鲜肉, 成猫, 幼猫类似这些的词不用处理");
            prompt.AppendLine("2. 实付金额, 格式: 精确到小数点后两位, 如果图片上只有一位, 则用0补足, 例如: 14.30");
            prompt.AppendLine("3. 下单时间, 格式: yyyy-MM-dd HH:mm:ss,例如: 2026-03-03 17:55:34");
            prompt.AppendLine("最终返回给我一个json格式:{\"Name\": \"魔宝蓝莓兔脆脆乐主食冻干100g * 1\",\"PurchasedAt\": \"2023-08-01 14:30:00\", \"FinalPrice\": 14.30}");

            var content = new Content
            {
                Parts = new List<Part>
                {
                    imagePart,
                    new Part { Text =  prompt.ToString()}
                }
            };
            try
            {

                var response = client.Models.GenerateContentAsync(
                                        model: "gemini-3-flash-preview",
                                        contents: content
                                    ).Result;

                Console.WriteLine(response.Candidates[0].Content.Parts[0].Text);
            }
            catch (Exception ex)
            {

                throw;
            }

        }


        #endregion
        public async static Task<ImageBuffer> LoadImageBufferFromFileAsync(string filePath)
        {
            StorageFile file = await StorageFile.GetFileFromPathAsync(filePath);
            IRandomAccessStream stream = await file.OpenAsync(FileAccessMode.Read);
            BitmapDecoder decoder = await BitmapDecoder.CreateAsync(stream);
            SoftwareBitmap bitmap = await decoder.GetSoftwareBitmapAsync();

            if (bitmap == null)
            {
                return null;
            }

            // 修复：使用已存在的CreateForSoftwareBitmap方法
            return ImageBuffer.CreateForSoftwareBitmap(bitmap);
        }

        public static async Task<ImageBuffer> OCRAsync()
        {
            string imgPath = @"D:\Computer\Projects\Lemon.Tools\FormTest\Photos\Screenshot1.jpg";
            StorageFile file = await StorageFile.GetFileFromPathAsync(imgPath);
            IRandomAccessStream stream = await file.OpenAsync(FileAccessMode.Read);
            BitmapDecoder decoder = await BitmapDecoder.CreateAsync(stream);
            SoftwareBitmap bitmap = await decoder.GetSoftwareBitmapAsync();

            if (bitmap == null)
            {
                return null;
            }

            return ImageBuffer.CreateForSoftwareBitmap(bitmap);
        }

        public async static Task<string> RecognizeTextFromSoftwareBitmap(ImageBuffer bitmap)
        {
            TextRecognizer textRecognizer = await EnsureModelIsReady1();
            //ImageBuffer imageBuffer = ImageBuffer.CreateForSoftwareBitmap(bitmap);
            ImageBuffer imageBuffer = bitmap;
            RecognizedText recognizedText = textRecognizer.RecognizeTextFromImage(imageBuffer);
            StringBuilder stringBuilder = new StringBuilder();

            foreach (var line in recognizedText.Lines)
            {
                stringBuilder.AppendLine(line.Text);
            }

            return stringBuilder.ToString();
        }

        public async static Task<TextRecognizer> EnsureModelIsReady1()
        {
            if (TextRecognizer.GetReadyState() == AIFeatureReadyState.NotReady)
            {
                var loadResult = await TextRecognizer.EnsureReadyAsync();
                if (loadResult.Status != AIFeatureReadyResultState.Success)
                {
                    // 修复：ExtendedError 是属性，不是方法
                    throw new Exception(loadResult.ExtendedError.Message);
                }
            }

            return await TextRecognizer.CreateAsync();
        }




	public void VisualizeWordBoundariesOnGrid(SoftwareBitmap bitmap,Grid grid,TextRecognizer textRecognizer)
    {
        ImageBuffer imageBuffer = ImageBuffer.CreateForSoftwareBitmap(bitmap);
        RecognizedText result = textRecognizer.RecognizeTextFromImage(imageBuffer);

        SolidColorBrush greenBrush = new SolidColorBrush(Microsoft.UI.Colors.Green);
        SolidColorBrush yellowBrush = new SolidColorBrush(Microsoft.UI.Colors.Yellow);
        SolidColorBrush redBrush = new SolidColorBrush(Microsoft.UI.Colors.Red);

        foreach (var line in result.Lines)
        {
            foreach (var word in line.Words)
            {
                PointCollection points = new PointCollection();
                var bounds = word.BoundingBox;
                points.Add(bounds.TopLeft);
                points.Add(bounds.TopRight);
                points.Add(bounds.BottomRight);
                points.Add(bounds.BottomLeft);

                Polygon polygon = new Polygon();
                polygon.Points = points;
                polygon.StrokeThickness = 2;

                if (word.MatchConfidence < 0.33)
                {
                    polygon.Stroke = redBrush;
                }
                else if (word.MatchConfidence < 0.67)
                {
                    polygon.Stroke = yellowBrush;
                }
                else
                {
                    polygon.Stroke = greenBrush;
                }

                grid.Children.Add(polygon);
            }
        }
    }


		public static void TestConnectionStr()
		{
			// 使用你的连接字符串（替换YourDatabaseName）
			var connectionString = "Server=192.168.51.117,1433;Database=DEV;User Id=sa;Password=Csf19961209==;Trusted_Connection=false;MultipleActiveResultSets=true;";

			try
			{
				using (var connection = new SqlConnection(connectionString))
				{
					connection.Open();
					Console.WriteLine("连接成功！");

					// 测试简单查询
					var command = new SqlCommand("SELECT @@VERSION", connection);
					var result = command.ExecuteScalar();
					Console.WriteLine($"SQL Server版本: {result}");
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"连接失败: {ex.Message}");
			}
		}

		public static void RecognizeLongImage(string imagePath)
		{
			var engine = new PaddleOCREngine();
			using var fullImage = new Bitmap(imagePath);
			List<PaddleOCRSharp.TextBlock> allTextBlocks = new List<PaddleOCRSharp.TextBlock>();
			const int segmentHeight = 1000; // 每段高度

			Stopwatch stopwatch = Stopwatch.StartNew();

			// 垂直分割图像
			for (int y = 0; y < fullImage.Height; y += segmentHeight)
			{
				int height = Math.Min(segmentHeight, fullImage.Height - y);
				using var segment = new Bitmap(fullImage.Width, height);

				using (var g = Graphics.FromImage(segment))
				{
					g.DrawImage(fullImage, new System.Drawing.Rectangle(0, 0, segment.Width, height),
								new System.Drawing.Rectangle(0, y, fullImage.Width, height), GraphicsUnit.Pixel);
				}

				// 识别片段
				var ocrResult = engine.DetectText(segment);
				if (ocrResult != null && ocrResult.TextBlocks != null)
				{
					// 关键修复：调整坐标偏移量
					foreach (var block in ocrResult.TextBlocks)
					{
						if (block.Score > 0.8)
						{
							// 调整Y坐标偏移
							var adjustedPoints = block.BoxPoints.Select(p =>
								new OCRPoint(p.X, p.Y + y)).ToList();

							// 创建新的文本块副本（保留原始属性）
							var adjustedBlock = new PaddleOCRSharp.TextBlock
							{
								Text = block.Text,
								Score = block.Score,
								BoxPoints = adjustedPoints
								// 添加其他必要属性...
							};

							allTextBlocks.Add(adjustedBlock);
						}
					}
				}
			}

			stopwatch.Stop();
			var groups = SmartGroupTextBlocks(allTextBlocks);

			// 获取运行时间
			TimeSpan elapsed = stopwatch.Elapsed;
			Console.WriteLine($"运行时间: {elapsed.TotalSeconds:0.000} 秒");
			Console.WriteLine($"识别完成！共识别 {allTextBlocks.Count} 个文本块");
		}

		public static List<Trade> SmartGroupTextBlocks(List<PaddleOCRSharp.TextBlock> textBlocks)
		{
			if (textBlocks == null || textBlocks.Count == 0)
				return new List<Trade>();

			// 1. 计算文本块高度统计值
			var heights = textBlocks.Select(block =>
			{
				var yValues = block.BoxPoints.Select(p => p.Y).ToList();
				return yValues.Max() - yValues.Min();
			}).ToList();

			// 2. 计算平均高度
			var avgHeight = heights.Average();

			// 3. 动态设置阈值（平均高度的30%）
			var dynamicThreshold = avgHeight * 0.3f;
			dynamicThreshold = 35d;
			// 4. 使用动态阈值分组
			return GroupTextBlocksByLine(textBlocks, dynamicThreshold);
		}

		public static List<Trade> GroupTextBlocksByLine(List<PaddleOCRSharp.TextBlock> textBlocks, double yThreshold = 10f)
		{
			if (textBlocks == null || textBlocks.Count == 0)
				return new List<Trade>();

			// 计算每个文本块的Y坐标参考值
			var blocksWithY = textBlocks.Select(block =>
			{
				var yValues = block.BoxPoints.Select(p => p.Y).ToList();
				return new
				{
					Block = block,
					MinY = yValues.Min(),
					MaxY = yValues.Max(),
					CenterY = (yValues.Min() + yValues.Max()) / 2f
				};
			}).ToList();

			// 按Y坐标排序
			blocksWithY = blocksWithY.OrderBy(b => b.CenterY).ToList();

			var groups = new List<List<PaddleOCRSharp.TextBlock>>();
			var currentGroup = new List<PaddleOCRSharp.TextBlock>();
			Trade trade = null;
			var trades = new List<Trade>();
			float? lastCenterY = null;

			foreach (var block in blocksWithY)
			{
				if (lastCenterY == null || Math.Abs(block.CenterY - lastCenterY.Value) <= yThreshold)
				{
					// 属于当前行
					currentGroup.Add(block.Block);
					lastCenterY = block.CenterY; // 更新为当前块的Y值
				}
				else
				{
					// 新行开始, 处理当前组为trade
					currentGroup = currentGroup.OrderBy(b => b.BoxPoints[0].X).ToList();
					if (currentGroup.ElementAtOrDefault(0).Text == "买金" || currentGroup.ElementAtOrDefault(0).Text == "卖金")
					{
						trade = new Trade
						{
							Type = currentGroup.ElementAtOrDefault(0).Text,
							Status = currentGroup.ElementAtOrDefault(1).Text.Contains("成功"),
							OperatedAt = DateTime.TryParse($"2025-{currentGroup.ElementAtOrDefault(2).Text.Insert(5, " ")}", out var operatedAt) ? operatedAt : DateTime.MinValue,
							Weight = float.TryParse(currentGroup.ElementAtOrDefault(4).Text.Replace("克", ""), out var weight) ? weight : 0F,
							UnitPrice = Decimal.TryParse(currentGroup.ElementAtOrDefault(5).Text.Replace("元", ""), out var unitPrice) ? unitPrice : 0M,
							TotalPrice = Decimal.TryParse(currentGroup.ElementAtOrDefault(6).Text.Replace("元", ""), out var totalPrice) ? totalPrice : 0M,
							Fee = Decimal.TryParse(currentGroup.Count == 8 ? currentGroup.ElementAtOrDefault(7).Text.Replace("元", "") : string.Empty, out var fee) ? fee : 0M,

						};
						trades.Add(trade);
					}

					//groups.Add(new List<PaddleOCRSharp.TextBlock>(currentGroup));
					currentGroup.Clear();
					currentGroup.Add(block.Block);
					lastCenterY = block.CenterY;
				}
			}

			if (currentGroup.Count > 0)
			{
				groups.Add(currentGroup);
			}

			return trades;
		}

		public class Trade
		{
			public string Type { get; set; }
			public DateTime OperatedAt { get; set; }
			public float Weight { get; set; }
			public decimal UnitPrice { get; set; }
			public decimal TotalPrice { get; set; }
			public decimal Fee { get; set; }
			public bool Status { get; set; }
		}

		public static string GetPicContent()
		{
			string imgPath = @"D:\Computer\Projects\Lemon.Tools\FormTest\Photos\Screenshot1.jpg";


			var ocr = new PaddleOCREngine();


			OCRResult ocrResult = new PaddleOCREngine().DetectText(imgPath);
			if (ocrResult != null)
				return ocrResult.Text;
			return string.Empty;


		}



		public static async Task<string> RecognizeText()
		{
			string imgPath = @"D:\Computer\Projects\Lemon.Tools\FormTest\Photos\Screenshot1.jpg";
			StorageFile file = await StorageFile.GetFileFromPathAsync(imgPath);
			IRandomAccessStream stream = await file.OpenAsync(FileAccessMode.Read);
			BitmapDecoder decoder = await BitmapDecoder.CreateAsync(stream);
			SoftwareBitmap bitmap = await decoder.GetSoftwareBitmapAsync();
			var textRecognizer = await EnsureModelIsReady();
			var imageBuffer = ImageBuffer.CreateForSoftwareBitmap(bitmap);
			var recognizedText = textRecognizer.RecognizeTextFromImage(imageBuffer);
			var stringBuilder = new StringBuilder();
			foreach (var line in recognizedText.Lines)
			{
				stringBuilder.AppendLine(line.Text);
			}
			return stringBuilder.ToString();
		}

		public static async Task<TextRecognizer> EnsureModelIsReady()
		{
			if (TextRecognizer.GetReadyState() == AIFeatureReadyState.Ready)
			{
				var loadResult = await TextRecognizer.EnsureReadyAsync();
				if (loadResult.Status != AIFeatureReadyResultState.Success)
				{
					throw loadResult.ExtendedError;
				}
				return await TextRecognizer.CreateAsync();
			}
			return await TextRecognizer.CreateAsync();

		}

		public static int SumFrom1toX(int x)
		{
			if (x == 1)
			{
				return 1;
			}
			else
			{
				return x + SumFrom1toX(x - 1);
			}
		}

		//public static async Task<decimal> GetCmbcAccumulatedGoldPrice()
		//{
		//	using (var client = new HttpClient())
		//	{
		//		// 民生银行积存金页面（示例URL，需确认实际地址）
		//		//var html = await client.GetAsync("https://www.cngold.org/img_date/bank_gold.html");
		//		var page = ScrapeLazyLoadedData();//await html.Content.ReadAsStringAsync();
		//										  // 使用HtmlAgilityPack解析
		//		var doc = new HtmlDocument();
		//		doc.LoadHtml(page);

		//		// 通过XPath定位价格（需根据实际网页结构调整）
		//		var node = doc.DocumentNode.SelectSingleNode("//td[@class='JO_283982q63']");
		//		return decimal.Parse(node.ChildNodes.First().InnerText.Trim());
		//	}
		//}

		//public static string ScrapeLazyLoadedData()
		//{
		//	var options = new ChromeOptions();
		//	options.AddArgument("--headless"); // 无头模式
		//	using (var driver = new ChromeDriver(options))
		//	{
		//		driver.Navigate().GoToUrl("https://www.cngold.org/img_date/bank_gold.html");

		//		// 模拟滚动到底部 5 次
		//		for (int i = 0; i < 5; i++)
		//		{
		//			((IJavaScriptExecutor)driver).ExecuteScript(
		//				"window.scrollTo(0, document.body.scrollHeight);");
		//			Thread.Sleep(2000); // 等待数据加载
		//		}

		//		// 获取完整页面源码
		//		return driver.PageSource;
		//		// 使用 HtmlAgilityPack 解析数据...
		//	}
		//}

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
			var name = groups[3].Value.Replace("猎粑", "猫粮").Replace("内", "肉");
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
			//var picPath = @"V:\Screenshots\IMG_5816.PNG";
			string picPath = @"D:\Computer\Projects\Lemon.Tools\FormTest\Photos\Screenshot1.jpg";
			//var picPath = @"D:\Computer\Projects\Samples\OCR-tesseract\tesseract-samples\src\Tesseract.ConsoleDemo\IMG_5783.PNG";
			var tessdata = @"D:\Computer\Projects\Lemon.Tools\OcrApi\tessdata";
			var ocrHelper = new OCRHelper(tessdata);
			picPath = ocrHelper.ReduceImageNoise(picPath);
			var content = ocrHelper.GetTextFromPicture(picPath, Languages.Chinese_Simplified, Tesseract.EngineMode.TesseractAndLstm).Replace(" ", string.Empty);

			Console.WriteLine(content);
		}

		public static void TestHttpClientHelper()
		{
			////GET https://jsonplaceholder.typicode.com/todos?userId=1&completed=false
			//var httpClientFactory = ServiceHelper.GetHttpClientFactory();
			//var httpHelper = new HttpClientHelper(httpClientFactory);
			//httpHelper.SetBaseAddress("https://jsonplaceholder.typicode.com/todos");
			//var @params = new Dictionary<string, object>
			//{
			//	{ "userId" , 1 },
			//	{ "completed" , false},
			//};
			//httpHelper.AddParameters(@params);
			//var response = httpHelper.GetAsync(string.Empty).Result;
			//Console.WriteLine(response);
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
			var result = sqlHelper.Db.Table<Student>().FirstOrDefault(student => student.Name == "Mike");
			Console.WriteLine($"Student {result.Name}'s Id is {result.Id}.");
		}

		//public static async Task RunGeminiOcrServiceTest()
		//{
		//	try
		//	{
		//		var sqlHelper = new SQLiteHelper();
		//		sqlHelper.SetConnectionString(@".\catfood_test.db");
		//		var repo = new CatFoodManager.Core.Repositories.CommonRepository(sqlHelper);

		//		var aiConfig = new Twotwo.Agent.Configuration.AIConfig
		//		{
		//			ApiKey = "",
		//			ModelName = "gemini-2.5-flash",
		//			Proxy = new Twotwo.Agent.Configuration.ProxyConfig { Enabled = true, Address = "http://127.0.0.1:10808" }
		//		};

		//		// Ensure prompts are loaded by caller if needed. Use a simple prompt here.
		//		var prompt = "请识别图片中的购买信息并返回 JSON 列表，每项包含 Name, PurchasedAt(yyyy-MM-dd HH:mm:ss), FinalPrice";

		//		var service = await CatFoodManager.Core.Services.GeminiOcrService.CreateAsync(repo, aiConfig);

		//		var folder = @"C:\Users\Lemony\Desktop"; // adjust if needed
		//		var results = await service.ProcessPicAsync<BestPrice>(folder, prompt);

		//		Console.WriteLine("Parsed DTOs:");
		//		foreach (var r in results)
		//		{
		//			Console.WriteLine($"Name:{r.Name}, PurchasedAt:{r.PurchasedAt}, FinalPrice:{r.FinalPrice}");
		//		}

		//		// verify DB insertion
		//		var saved = repo.QueryList<CatFoodManager.Core.Models.GeminiResponseEntity>().ToList();
		//		Console.WriteLine($"Saved GeminiResponseEntity count: {saved.Count}");
		//	}
		//	catch (Exception ex)
		//	{
		//		Console.WriteLine($"RunGeminiOcrServiceTest failed: {ex.Message}");
		//	}
		//}

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
		[PrimaryKey, Column("id")]
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
