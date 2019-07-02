namespace TestOkur.Notification.Infrastructure
{
	using System.Threading.Tasks;

	public interface ITemplateService
	{
		Task<string> RenderTemplateAsync<TViewModel>(string filePath, TViewModel viewModel);
	}
}