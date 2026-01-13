using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.Versioning;
using System.Windows.Forms;

namespace ScreenshotTools
{
	[SupportedOSPlatform("windows")]
	public class ScreenshotHelper
	{
		[SupportedOSPlatform("windows")]
		public void TakeScreenshot(string imageSaveUrl, out string localFilePath)
		{
			// Get the size of the screen
			Rectangle bounds = Screen.PrimaryScreen.Bounds;

			// Create a Bitmap object to capture the screenshot
			using Bitmap bitmap = new Bitmap(bounds.Width, bounds.Height);
			using Graphics g = Graphics.FromImage(bitmap);
			// Capture the screen
			g.CopyFromScreen(Point.Empty, Point.Empty, bounds.Size);

			// Save the screenshot to a file
			string fileName = DateTime.Now.ToString("yyyy-MM-dd") + ".png";
			localFilePath = Path.Combine(imageSaveUrl, "screenshot.png");
			bitmap.Save(localFilePath, ImageFormat.Png);
		}

		//to do draw rectangle
	}
}
