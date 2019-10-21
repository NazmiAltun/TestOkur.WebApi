namespace TestOkur.Notification.Infrastructure
{
    using System.IO;
    using System.Threading.Tasks;
    using RazorLight;

    public class TemplateService : ITemplateService
    {
        private readonly IRazorLightEngine _engine;

        public TemplateService(IRazorLightEngine engine)
        {
            _engine = engine;
        }

        public async Task<string> RenderTemplateAsync<TViewModel>(string filePath, TViewModel viewModel)
        {
            var template = File.ReadAllText(Path.Combine("Templates", filePath));
            var name = string.Join('_', Path.Combine("Templates", filePath)
                .Replace('.', '_')
                .Split(Path.PathSeparator));

            return await _engine.CompileRenderAsync(name, template, viewModel);
        }
    }
}
