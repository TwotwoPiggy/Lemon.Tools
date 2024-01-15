using AutoCursorTool;
using Azure.Storage.Blobs;
using CommonTools;
using GptApi;
using GptApi.Models;
using HttpManager;
using ScreenshotTools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using static AutoCursorTool.User32Input;

namespace FormTest
{
	public partial class FormTestAutoCursor : Form
	{
		private readonly CursorHelper _cursorHelper = new CursorHelper();
		private int _handleNum = 0;

		public FormTestAutoCursor()
		{
			InitializeComponent();
		}

		private void btnMoveCursor_Click(object sender, EventArgs e)
		{
			var timerManager = new TimerManager(2);
			_cursorHelper.InitCursor(1, 1);

			timerManager.SetTimer(KeepActiveEvent, default);
			timerManager.EnableTimer();
		}

		private void KeepActiveEvent(Object source, ElapsedEventArgs e)
		{
			if (_handleNum >= 50)
			{
				_cursorHelper.InitCursor(1, 1);
				_handleNum = 0;
			}
			//_cursorHelper.MoveCursor(10, 10);
			//_cursorHelper.ClickMouse(MouseEvent_LeftUp | MouseEvent_LeftDown, 0, 0, 0, 0);
			_handleNum++;
		}

		private void TakeScreenshotEvent(Object source, ElapsedEventArgs e)
		{
			var screenshotHelper = new ScreenshotHelper();
			var localFilePath = string.Empty;
			screenshotHelper.TakeScreenshot(@"D:\Computers\develop\.NetCore\0.Lemon\FormTest\Photos", out localFilePath);
			var connectionString = "DefaultEndpointsProtocol=https;AccountName=screenshot12;AccountKey=oPpVrxNJLytxUNikr8+eD3syd4LL4KAs4wCTu+pT0B0/tnZZO9eVTEr8rl/lOvTdU1lsZTjopMVD+ASts3N0Jw==;EndpointSuffix=core.windows.net";
			var blobContainerName = "screenshot";
			var containerClient = new BlobContainerClient(connectionString, blobContainerName);
			UploadStreamAsync(containerClient, localFilePath);
		}

		public static async Task UploadFile(BlobContainerClient containerClient, string localFilePath)
		{
			string fileName = Path.GetFileName(localFilePath);
			BlobClient blobClient = containerClient.GetBlobClient(fileName);

			await blobClient.UploadAsync(localFilePath, true);
		}

		public static async Task UploadStreamAsync(BlobContainerClient containerClient, string localFilePath)
		{
			string fileName = Path.GetFileName(localFilePath);
			BlobClient blobClient = containerClient.GetBlobClient(fileName);

			FileStream fileStream = File.OpenRead(localFilePath);
			await blobClient.UploadAsync(fileStream, true);
			fileStream.Close();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			var timerManager = new TimerManager(2);
			timerManager.SetTimer(TakeScreenshotEvent, 5, false);
			timerManager.EnableTimer();
		}

		private void button2_Click(object sender, EventArgs e)
		{
			using (var client = new HttpClient())
			{
				var httpRequest = new HttpRequest(client);
				var apiKey = "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";
				httpRequest.AddAuthorization(apiKey);
				var gpt = new Gpt3Api(httpRequest, apiKey);
				var endpoint = @"https://api.openai.com/v1/chat/completions";
				var message = new GptApi.Models.Message
				{
					Role = Role.User,
					Content = "Hello"
				};

				var request = new Request()
				{
					Model = GptModel.GPT3_5_turbo,
					Messages = new List<GptApi.Models.Message> { message }
				};
				var result = gpt.GenerateResponseAsync(endpoint, request);
			}

		}

		
		private void button3_Click(object sender, EventArgs e)
		{
			SystemManager.ShutDownMachine(30 * 60);
		}

		private void button4_Click(object sender, EventArgs e)
		{
			SystemManager.Cancel();
		}
	}
}
