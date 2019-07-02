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
		private const string FontFamily = "Ubuntu";
		private const int Length = 4;
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

			return BuildImage(code, 50, 100, 20, 5);
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

		private MemoryStream BuildImage(string captchaCode, int imageHeight, int imageWidth, int fontSize, int distortion = 18)
		{
			var memoryStream = new MemoryStream();

			using (var captchaImage = new Bitmap(imageWidth, imageHeight))
			{
				using (var cache = new Bitmap(imageWidth, imageHeight))
				{
					using (var graphicsTextHolder = Graphics.FromImage(captchaImage))
					{
						graphicsTextHolder.Clear(Color.Wheat);
						graphicsTextHolder.DrawString(
							captchaCode,
							new Font(FontFamily, fontSize, FontStyle.Italic),
							new SolidBrush(Color.Gray),
							new PointF(8.4F, 10.4F));

						for (var y = 0; y < imageHeight; y++)
						{
							for (var x = 0; x < imageWidth; x++)
							{
								var newX = (int)(x + (distortion * Math.Sin(Math.PI * y / 64.0)));
								var newY = (int)(y + (distortion * Math.Cos(Math.PI * x / 64.0)));
								if (newX < 0 || newX >= imageWidth)
								{
									newX = 0;
								}

								if (newY < 0 || newY >= imageHeight)
								{
									newY = 0;
								}

								cache.SetPixel(x, y, captchaImage.GetPixel(newX, newY));
							}
						}

						cache.Save(memoryStream, ImageFormat.Png);
						memoryStream.Position = 0;
						return memoryStream;
					}
				}
			}
		}

		private string GenerateCode(int length)
		{
			return new string(Enumerable.Repeat(Letters, length)
				.Select(s => s[_random.Next(s.Length)]).ToArray());
		}
	}
}
