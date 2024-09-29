using AutoCursorTool;
using Azure.Storage.Blobs;
using CommonTools;
using ScreenshotTools;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

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
			screenshotHelper.TakeScreenshot(@"D:\Computer\Projects\Lemon.Tools\FormTest\Photos", out localFilePath);
			var connectionString = "DefaultEndpointsProtocol=https;AccountName=screenshot12;AccountKey=oPpVrxNJLytxUNikr8+eD3syd4LL4KAs4wCTu+pT0B0/tnZZO9eVTEr8rl/lOvTdU1lsZTjopMVD+ASts3N0Jw==;EndpointSuffix=core.windows.net";
			var blobContainerName = "screenshot";
			var containerClient = new BlobContainerClient(connectionString, blobContainerName);
			UploadStreamAsync(containerClient, localFilePath).ConfigureAwait(false);
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
			//using (var client = new HttpClient())
			//{
			//	var httpRequest = new HttpClientHelper(client);
			//	var apiKey = "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";
			//	httpRequest.SetAuthorization("Bearer",apiKey);
			//	var gpt = new Gpt3Api(httpRequest, apiKey);
			//	var endpoint = @"https://api.openai.com/v1/chat/completions";
			//	var message = new GptApi.Models.Message
			//	{
			//		Role = Role.User,
			//		Content = "Hello"
			//	};

			//	var request = new Request()
			//	{
			//		Model = GptModel.GPT3_5_turbo,
			//		Messages = new List<GptApi.Models.Message> { message }
			//	};
			//	var result = gpt.GenerateResponseAsync(endpoint, request);
			//}

		}


		private void button3_Click(object sender, EventArgs e)
		{
			SystemManager.ShutDownMachine(minutes: 10);
		}

		private void button4_Click(object sender, EventArgs e)
		{
			SystemManager.Cancel();
		}

		private void button5_Click(object sender, EventArgs e)
		{
			var dir = @"\\SEAGATE-D2\OneTwoNas\Films";
			var result = FileManager.IsOrExistDirectory(dir);
			var sourcePath = @"\\SEAGATE-D2\OneTwoNas\Films\test2.txt";
			var targetPath = @"\\SEAGATE-D2\OneTwoNas\Films\test3.txt";
			result = FileManager.IsOrExistFile(sourcePath);
			FileManager.RenameFile(sourcePath, targetPath);
		}

		private void button6_Click(object sender, EventArgs e)
		{
			this.smoothProgressBar1.Value = 0;

			this.timer1.Interval = 1;
			this.timer1.Enabled = true;
		}

		private void timer1_Tick(object sender, EventArgs e)
		{
			if (this.smoothProgressBar1.Value < 100)
			{
				this.smoothProgressBar1.Value++;
			}
			else
			{
				this.timer1.Enabled = false;
			}
		}

		private void button7_Click(object sender, EventArgs e)
		{
			pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
			var qrCode = QRCodeHelper.GenerateQRCode("Test", this.pictureBox1.Size.Height);
			this.pictureBox1.Image = qrCode;
		}
	}
}
