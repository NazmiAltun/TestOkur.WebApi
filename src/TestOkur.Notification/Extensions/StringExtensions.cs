namespace TestOkur.Notification.Extensions
{
    using System;

    internal static class StringExtensions
	{
		public static string ToSmsFriendly(this string message)
		{
			return string.IsNullOrEmpty(message)
				? string.Empty
				: message.Replace('ö', 'o')
					.Replace(Environment.NewLine, string.Empty)
					.Replace('ç', 'c')
					.Replace('ş', 's')
					.Replace('ğ', 'g')
					.Replace('ü', 'u')
					.Replace('Ç', 'C')
					.Replace('Ş', 'S')
					.Replace('Ğ', 'G')
					.Replace('Ü', 'U')
					.Replace('Ö', 'O')
					.Replace('İ', 'I')
					.Replace('ı', 'i')
					.Replace("&#220;", "U")
					.Replace("&#214;", "O")
					.Replace("&#199;", "C")
					.Replace("&#252;", "u")
					.Replace("&#246;", "o")
					.Replace("&#231;", "c");
		}
	}
}
