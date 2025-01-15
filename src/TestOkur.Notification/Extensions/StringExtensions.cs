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
                    .Replace("@", "|01|")
                    .Replace("£", "|02|")
                    .Replace("$", "|03|")
                    .Replace("€", "|05|")
                    .Replace("_", "|14|")
                    .Replace("!", "|26|")
                    .Replace("'", "|27|")
                    .Replace("#", "|28|")
                    .Replace("%", "|30|")
                    .Replace("&", "|31|")
                    .Replace("(", "|33|")
                    .Replace(")", "|34|")
                    .Replace("*", "|35|")
                    .Replace("+", "|36|")
                    .Replace("-", "|38|")
                    .Replace("/", "|39|")
                    .Replace(":", "|40|")
                    .Replace(";", "|41|")
                    .Replace("<", "|42|")
                    .Replace("=", "|43|")
                    .Replace(">", "|44|")
                    .Replace("?", "|45|")
                    .Replace("{", "|46|")
                    .Replace("}", "|47|")
                    .Replace("~", "|49|")
                    .Replace("^", "|51|")
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
