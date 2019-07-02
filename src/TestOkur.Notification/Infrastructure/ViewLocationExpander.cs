namespace TestOkur.Notification.Infrastructure
{
	using System.Collections.Generic;
	using System.Linq;
	using Microsoft.AspNetCore.Mvc.Razor;

	public class ViewLocationExpander : IViewLocationExpander
	{
		public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context, IEnumerable<string> viewLocations)
		{
			var locationWithoutController = "/Templates/{0}.cshtml";
			return viewLocations.Union(new[] { locationWithoutController });
		}

		public void PopulateValues(ViewLocationExpanderContext context)
		{
			// Do nothing
		}
	}
}
