namespace TestOkur.WebApi.Application.Captcha
{
    using System;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Linq;
    using CacheManager.Core;

    public class CaptchaService : ICaptchaService
    {
        private const string Letters = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private const string FontFamily = "DejaVuSans";
        private const int Length = 4;
        private const int Distortion = 10;
        private const int FontSize = 20;
        private readonly ICacheManager<Captcha> _captchaCache;
        private readonly Random _random = new Random();

        public CaptchaService(ICacheManager<Captcha> captchaCache)
        {
            _captchaCache = captchaCache;
        }

        public Stream Generate(Guid id)
        {
            var code = GenerateCode(Length);

            _captchaCache.Add(new CacheItem<Captcha>(
                $"Captcha_{id}",
                new Captcha(id, code),
                ExpirationMode.Absolute,
                TimeSpan.FromHours(1)));

            return BuildImage(code, 50, 100);
        }

        public bool Validate(Guid id, string code)
        {
            if (id == Guid.Empty)
            {
                return false;
            }

            var captcha = _captchaCache.Get($"Captcha_{id}");

            return captcha?.Code == code;
        }

        private MemoryStream BuildImage(string captchaCode, int imageHeight, int imageWidth)
        {
            var memoryStream = new MemoryStream();

            using (var captchaImage = new Bitmap(imageWidth, imageHeight))
            {
                using (var cache = new Bitmap(imageWidth, imageHeight))
                {
                    using (var graphicsTextHolder = Graphics.FromImage(captchaImage))
                    {
                        DrawCaptchaCode(captchaCode, graphicsTextHolder);
                        DistortImage(imageHeight, imageWidth, cache, captchaImage);
                        cache.Save(memoryStream, ImageFormat.Png);
                        memoryStream.Position = 0;
                        return memoryStream;
                    }
                }
            }
        }

        private void DistortImage(int imageHeight, int imageWidth, Bitmap cache, Bitmap captchaImage)
        {
            for (var y = 0; y < imageHeight; y++)
            {
                for (var x = 0; x < imageWidth; x++)
                {
                    var newX = Distort(x, y, imageWidth);
                    var newY = Distort(y, x, imageHeight);

                    cache.SetPixel(x, y, captchaImage.GetPixel(newX, newY));
                }
            }
        }

        private void DrawCaptchaCode(string captchaCode, Graphics graphicsTextHolder)
        {
            graphicsTextHolder.Clear(Color.Wheat);
            graphicsTextHolder.DrawString(
                captchaCode,
                new Font(FontFamily, FontSize, FontStyle.Italic),
                new SolidBrush(Color.Gray),
                new PointF(8.4F, 10.4F));
        }

        private int Distort(int d1, int d2, int limit)
        {
            var newD = (int)(d1 + (Distortion * Math.Sin(Math.PI * d2 / 64.0)));
            if (newD < 0 || newD >= limit)
            {
                newD = 0;
            }

            return newD;
        }

        private string GenerateCode(int length)
        {
            return new string(Enumerable.Repeat(Letters, length)
                .Select(s => s[_random.Next(s.Length)]).ToArray());
        }
    }
}
