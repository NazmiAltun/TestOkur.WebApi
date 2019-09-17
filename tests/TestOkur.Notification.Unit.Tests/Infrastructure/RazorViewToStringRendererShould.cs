namespace TestOkur.Notification.Unit.Tests.Infrastructure
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using FluentAssertions;
    using RazorLight;
    using TestOkur.Notification.Infrastructure;
    using Xunit;

    public class RazorViewToStringRendererShould
	{
		private readonly TemplateService _render;

		public RazorViewToStringRendererShould()
		{
			var engine = new RazorLightEngineBuilder()
				.UseMemoryCachingProvider()
				.Build();

			_render = new TemplateService(engine);
		}

		[Fact]
		public async Task RenderTemplate()
		{
			var fileName = await GenerateTemplateAsync();
			var output = await _render.RenderTemplateAsync(fileName, new Foo("John", 30));

			output.Should().Contain("<tr><td>Name</td><td>John</td></tr>");
			output.Should().Contain("<tr><td>TotalSmsCount</td><td>30</td></tr>");
		}

		private async Task<string> GenerateTemplateAsync()
		{
			var template = @"<table>
								<tr><td>Name</td><td>@Model.Name</td></tr>
								<tr><td>TotalSmsCount</td><td>@Model.TotalSmsCount</td></tr>
									 </table>";
			var fileName = $"{Guid.NewGuid():N}.html";
			await File.WriteAllTextAsync(
				Path.Combine("Templates", fileName),
				template);

			return fileName;
		}

		public class Foo
		{
			public Foo(string name, int totalSmsCount)
			{
				Name = name;
				TotalSmsCount = totalSmsCount;
			}

			public string Name { get; }

			public int TotalSmsCount { get; }
		}
	}
}
