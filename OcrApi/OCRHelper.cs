using CommonTools;
using OcrApi.Models;
using OpenCvSharp;
using Tesseract;

namespace OcrApi
{
	public class OCRHelper
	{
		private string _tessdataPath = string.Empty;
		public string TessdataPath => _tessdataPath;

		public OCRHelper(string tessdataPath)
		{
			_tessdataPath = tessdataPath;
		}

		public void SetTessdataPath(string tessdataPath)
		{
			ArgumentNullException.ThrowIfNullOrWhiteSpace(tessdataPath, "Please provide a valid tessdataPath");
			_tessdataPath = tessdataPath;
		}


		public string GetTextFromPicture(string picturePath, string language = Languages.Chinese_Simplified, EngineMode engineMode = EngineMode.Default)
		{
			var resultContent = string.Empty;

			if (string.IsNullOrWhiteSpace(_tessdataPath) || !FileManager.IsOrExistDirectory(_tessdataPath))
			{
				new ArgumentException("Please set a valid tessdataPath by using method SetTessdataPath(string tessdataPath)", _tessdataPath);
			}
			if (string.IsNullOrWhiteSpace(picturePath) || !FileManager.IsOrExistFile(picturePath))
			{
				new ArgumentException("Please provide a valid picturePath", picturePath);
			}
			try
			{
				using var engine = new TesseractEngine(_tessdataPath, language, engineMode);
				using var pixImg = Pix.LoadFromFile(picturePath);
				using var page = engine.Process(pixImg);
				resultContent = page.GetText();
				return resultContent;
			}
			catch (Exception)
			{
				throw;
			}
		}

		/// <summary>
		/// 图片预处理: 降噪
		/// </summary>
		/// <param name="picturePath"></param>
		/// <param name="imreadModes"></param>
		/// <param name=""></param>
		/// <returns></returns>
		public string ReduceImageNoise(string picturePath, string? newPicturePath = null, ImreadModes imreadModes = ImreadModes.Grayscale, double thresh = 140, double maxval = 255, ThresholdTypes thresholdTypes = ThresholdTypes.Binary)
		{
			using var pictureMs = new FileStream(picturePath, FileMode.Open);
			var byteData = new byte[pictureMs.Length];
			pictureMs.Read(byteData, 0, byteData.Length);
			using var ms = new MemoryStream(byteData);
			var imgStream = Mat.FromStream(ms, imreadModes);
			//Cv2.ImShow("Input Image", simg);
			//阈值操作 阈值参数可以用一些可视化工具来调试得到
			var thresholdImg = imgStream.Threshold(thresh, maxval, thresholdTypes);
			//Cv2.ImShow("Threshold", ThresholdImg);
			newPicturePath = string.IsNullOrWhiteSpace(newPicturePath) ? $"{picturePath.GetPathWithOutExtension()}-new{picturePath.GetExtension()}" : newPicturePath;
			Cv2.ImWrite(newPicturePath, thresholdImg);
			return newPicturePath;
		}
	}
}
