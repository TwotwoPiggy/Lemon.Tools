using QRCoder;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonTools
{
	public static class QRCodeHelper
	{
		public static Bitmap GenerateQRCode(string payload,int size)
		{
			var qrCoder = new QRCodeGenerator();
			var qrCodeData = qrCoder.CreateQrCode(payload, QRCodeGenerator.ECCLevel.Q);
			using var qrCode = new PngByteQRCode(qrCodeData);
			var qrCodeAsPngByteArr = qrCode.GetGraphic(size);
			using var ms = new MemoryStream(qrCodeAsPngByteArr);
			return new Bitmap(ms);
		}
	}
}
