using QRCoder;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace CommonTools
{
    /// <summary>
    /// 二维码生成帮助类。
    /// <para>
    /// 基于 QRCoder 和 ImageSharp 实现，支持跨平台使用。
    /// </para>
    /// </summary>
    public static class QRCodeHelper
    {
        /// <summary>
        /// 生成二维码图像。
        /// </summary>
        /// <param name="payload">二维码内容。</param>
        /// <param name="pixelsPerModule">每个模块的像素数（默认为 10）。</param>
        /// <returns>二维码图像。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="payload"/> 为 null。</exception>
        /// <exception cref="ArgumentException"><paramref name="pixelsPerModule"/> 小于 1。</exception>
        public static Image GenerateQRCode(string payload, int pixelsPerModule = 10)
        {
            ArgumentNullException.ThrowIfNull(payload);

            if (pixelsPerModule < 1)
            {
                throw new ArgumentException("Pixels per module must be at least 1.", nameof(pixelsPerModule));
            }

            using var qrGenerator = new QRCodeGenerator();
            var qrCodeData = qrGenerator.CreateQrCode(payload, QRCodeGenerator.ECCLevel.Q);

            using var qrCode = new PngByteQRCode(qrCodeData);
            var qrCodeBytes = qrCode.GetGraphic(pixelsPerModule);

            return Image.Load<Rgb24>(qrCodeBytes);
        }

        /// <summary>
        /// 生成二维码图像并保存到文件。
        /// </summary>
        /// <param name="payload">二维码内容。</param>
        /// <param name="filePath">保存路径。</param>
        /// <param name="pixelsPerModule">每个模块的像素数（默认为 10）。</param>
        /// <exception cref="ArgumentNullException"><paramref name="payload"/> 或 <paramref name="filePath"/> 为 null。</exception>
        /// <exception cref="ArgumentException"><paramref name="pixelsPerModule"/> 小于 1。</exception>
        public static void GenerateQRCodeToFile(string payload, string filePath, int pixelsPerModule = 10)
        {
            ArgumentNullException.ThrowIfNull(filePath);

            using var image = GenerateQRCode(payload, pixelsPerModule);
            image.Save(filePath, new PngEncoder());
        }

        /// <summary>
        /// 异步生成二维码图像并保存到文件。
        /// </summary>
        /// <param name="payload">二维码内容。</param>
        /// <param name="filePath">保存路径。</param>
        /// <param name="pixelsPerModule">每个模块的像素数（默认为 10）。</param>
        /// <param name="cancellationToken">取消令牌。</param>
        /// <exception cref="ArgumentNullException"><paramref name="payload"/> 或 <paramref name="filePath"/> 为 null。</exception>
        /// <exception cref="ArgumentException"><paramref name="pixelsPerModule"/> 小于 1。</exception>
        public static async Task GenerateQRCodeToFileAsync(
            string payload,
            string filePath,
            int pixelsPerModule = 10,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(filePath);

            using var image = GenerateQRCode(payload, pixelsPerModule);
            await image.SaveAsync(filePath, new PngEncoder(), cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// 生成二维码 PNG 字节数组。
        /// </summary>
        /// <param name="payload">二维码内容。</param>
        /// <param name="pixelsPerModule">每个模块的像素数（默认为 10）。</param>
        /// <returns>PNG 格式的二维码字节数组。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="payload"/> 为 null。</exception>
        /// <exception cref="ArgumentException"><paramref name="pixelsPerModule"/> 小于 1。</exception>
        public static byte[] GenerateQRCodeBytes(string payload, int pixelsPerModule = 10)
        {
            ArgumentNullException.ThrowIfNull(payload);

            if (pixelsPerModule < 1)
            {
                throw new ArgumentException("Pixels per module must be at least 1.", nameof(pixelsPerModule));
            }

            using var qrGenerator = new QRCodeGenerator();
            var qrCodeData = qrGenerator.CreateQrCode(payload, QRCodeGenerator.ECCLevel.Q);

            using var qrCode = new PngByteQRCode(qrCodeData);
            return qrCode.GetGraphic(pixelsPerModule);
        }

        /// <summary>
        /// 生成带自定义颜色的二维码图像。
        /// </summary>
        /// <param name="payload">二维码内容。</param>
        /// <param name="pixelsPerModule">每个模块的像素数。</param>
        /// <param name="darkColor">暗色（二维码颜色）。</param>
        /// <param name="lightColor">亮色（背景颜色）。</param>
        /// <returns>二维码图像。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="payload"/> 为 null。</exception>
        /// <exception cref="ArgumentException"><paramref name="pixelsPerModule"/> 小于 1。</exception>
        public static Image GenerateQRCode(
            string payload,
            int pixelsPerModule,
            Rgb24 darkColor,
            Rgb24 lightColor)
        {
            ArgumentNullException.ThrowIfNull(payload);

            if (pixelsPerModule < 1)
            {
                throw new ArgumentException("Pixels per module must be at least 1.", nameof(pixelsPerModule));
            }

            using var qrGenerator = new QRCodeGenerator();
            var qrCodeData = qrGenerator.CreateQrCode(payload, QRCodeGenerator.ECCLevel.Q);

            var qrCode = new QRCode(qrCodeData);
            var moduleCount = qrCodeData.ModuleMatrix.Count;

            var size = moduleCount * pixelsPerModule;
            var image = new Image<Rgb24>(size, size, lightColor);

            image.Mutate(ctx =>
            {
                for (var x = 0; x < moduleCount; x++)
                {
                    for (var y = 0; y < moduleCount; y++)
                    {
                        if (qrCodeData.ModuleMatrix[y][x])
                        {
                            ctx.Fill(
                                new SolidBrush(darkColor),
                                new RectangleF(x * pixelsPerModule, y * pixelsPerModule, pixelsPerModule, pixelsPerModule));
                        }
                    }
                }
            });

            return image;
        }

        /// <summary>
        /// 生成带 Logo 的二维码图像。
        /// </summary>
        /// <param name="payload">二维码内容。</param>
        /// <param name="logoPath">Logo 文件路径。</param>
        /// <param name="pixelsPerModule">每个模块的像素数（默认为 10）。</param>
        /// <param name="logoSizeRatio">Logo 占二维码的比例（默认为 0.2，即 20%）。</param>
        /// <returns>带 Logo 的二维码图像。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="payload"/> 或 <paramref name="logoPath"/> 为 null。</exception>
        /// <exception cref="FileNotFoundException">Logo 文件不存在。</exception>
        /// <exception cref="ArgumentException"><paramref name="pixelsPerModule"/> 小于 1 或 <paramref name="logoSizeRatio"/> 不在有效范围内。</exception>
        public static Image GenerateQRCodeWithLogo(
            string payload,
            string logoPath,
            int pixelsPerModule = 10,
            double logoSizeRatio = 0.2)
        {
            ArgumentNullException.ThrowIfNull(logoPath);

            if (!File.Exists(logoPath))
            {
                throw new FileNotFoundException("Logo file not found.", logoPath);
            }

            using var logo = Image.Load(logoPath);
            return GenerateQRCodeWithLogo(payload, logo, pixelsPerModule, logoSizeRatio);
        }

        /// <summary>
        /// 生成带 Logo 的二维码图像。
        /// </summary>
        /// <param name="payload">二维码内容。</param>
        /// <param name="logo">Logo 图像。</param>
        /// <param name="pixelsPerModule">每个模块的像素数（默认为 10）。</param>
        /// <param name="logoSizeRatio">Logo 占二维码的比例（默认为 0.2，即 20%）。</param>
        /// <returns>带 Logo 的二维码图像。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="payload"/> 或 <paramref name="logo"/> 为 null。</exception>
        /// <exception cref="ArgumentException"><paramref name="pixelsPerModule"/> 小于 1 或 <paramref name="logoSizeRatio"/> 不在有效范围内。</exception>
        public static Image GenerateQRCodeWithLogo(
            string payload,
            Image logo,
            int pixelsPerModule = 10,
            double logoSizeRatio = 0.2)
        {
            ArgumentNullException.ThrowIfNull(logo);

            if (pixelsPerModule < 1)
            {
                throw new ArgumentException("Pixels per module must be at least 1.", nameof(pixelsPerModule));
            }

            if (logoSizeRatio <= 0 || logoSizeRatio >= 0.5)
            {
                throw new ArgumentException("Logo size ratio must be between 0 and 0.5 (exclusive).", nameof(logoSizeRatio));
            }

            using var qrGenerator = new QRCodeGenerator();
            var qrCodeData = qrGenerator.CreateQrCode(payload, QRCodeGenerator.ECCLevel.H);

            var moduleCount = qrCodeData.ModuleMatrix.Count;
            var size = moduleCount * pixelsPerModule;

            var darkColor = new Rgb24(0, 0, 0);
            var lightColor = new Rgb24(255, 255, 255);

            var image = new Image<Rgb24>(size, size, lightColor);

            image.Mutate(ctx =>
            {
                for (var x = 0; x < moduleCount; x++)
                {
                    for (var y = 0; y < moduleCount; y++)
                    {
                        if (qrCodeData.ModuleMatrix[y][x])
                        {
                            ctx.Fill(
                                new SolidBrush(darkColor),
                                new RectangleF(x * pixelsPerModule, y * pixelsPerModule, pixelsPerModule, pixelsPerModule));
                        }
                    }
                }

                var logoSize = (int)(size * logoSizeRatio);
                var logoX = (size - logoSize) / 2;
                var logoY = (size - logoSize) / 2;

                var resizedLogo = logo.Clone(ctx => ctx.Resize(logoSize, logoSize));
                ctx.DrawImage(resizedLogo, new Point(logoX, logoY), 1f);
            });

            return image;
        }

        /// <summary>
        /// 生成 Base64 编码的二维码数据 URI。
        /// </summary>
        /// <param name="payload">二维码内容。</param>
        /// <param name="pixelsPerModule">每个模块的像素数（默认为 10）。</param>
        /// <returns>Base64 数据 URI 字符串。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="payload"/> 为 null。</exception>
        public static string GenerateQRCodeDataUri(string payload, int pixelsPerModule = 10)
        {
            var bytes = GenerateQRCodeBytes(payload, pixelsPerModule);
            var base64 = Convert.ToBase64String(bytes);
            return $"data:image/png;base64,{base64}";
        }
    }
}
