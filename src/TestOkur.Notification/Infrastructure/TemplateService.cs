namespace TestOkur.Notification.Infrastructure
{
    using System.IO;
    using System.Threading.Tasks;
    using RazorLight;
    using TestOkur.Infrastructure.Mvc.Helpers;

    public class TemplateService : ITemplateService
    {
        private readonly IRazorLightEngine _engine;

        public TemplateService(IRazorLightEngine engine)
        {
            _engine = engine;
        }

        public async Task<string> RenderTemplateAsync<TViewModel>(string filePath, TViewModel viewModel)
        {
            var template = await FileEx.ReadAllTextAsync(Path.Combine("Templates", filePath));
            var name = Path.Combine("Templates", filePath)
                .Replace('.', '_')
                .Replace(Path.PathSeparator, '_')
                .Replace(Path.VolumeSeparatorChar, '_')
                .Replace(Path.AltDirectorySeparatorChar, '_')
                .Replace(Path.DirectorySeparatorChar, '_');

            return await _engine.CompileRenderAsync(name, template, viewModel);
        }
    }
}
