namespace TestOkur.WebApi.Extensions
{
    using Dapper.FluentMap.Configuration;
    using Dapper.FluentMap.Mapping;
    using Paramore.Brighter.Extensions;
    using System;
    using System.Linq;
    using System.Reflection;

    public static class FluentMapConfigurationExtensions
    {
        public static void AddMapFromCurrentAssembly(this FluentMapConfiguration configuration)
        {
            var method = configuration.GetType().GetMethod("AddMap");
            Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => typeof(IEntityMap).IsAssignableFrom(t))
                .Each(t =>
                {
                    method.MakeGenericMethod(t.GetInterfaces()
                            .First(i => i.IsGenericType)
                            .GetGenericArguments().First())
                        .Invoke(configuration, new[] { Activator.CreateInstance(t) });
                });
        }
    }
}
