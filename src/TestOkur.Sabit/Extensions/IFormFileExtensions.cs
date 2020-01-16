namespace TestOkur.Sabit.Extensions
{
    using Microsoft.AspNetCore.Http;
    using System.IO;
    using System.Threading.Tasks;

    public static class FormFileExtensions
    {
        public static async Task SaveAsync(this IFormFile file, string folderPath)
        {
            var path = Path.Combine(folderPath, file.FileName);

            await using var stream = new FileStream(path, FileMode.Create);
            await file.CopyToAsync(stream);
        }
    }
}
