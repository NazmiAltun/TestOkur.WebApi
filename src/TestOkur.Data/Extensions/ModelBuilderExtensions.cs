namespace Microsoft.EntityFrameworkCore
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;
	using TestOkur.Domain.SeedWork;

	internal static class ModelBuilderExtensions
    {
        public static void AddAuditableProperties(this ModelBuilder modelBuilder)
        {
            foreach (var type in GetAuditableTypes())
            {
	            modelBuilder.Entity(type).Property<int>("CreatedBy");
	            modelBuilder.Entity(type).Property<int>("UpdatedBy");
	            modelBuilder.Entity(type).Property<DateTime>("CreatedOnUTC");
	            modelBuilder.Entity(type).Property<DateTime>("UpdatedOnUTC");
            }
        }

        private static IEnumerable<Type> GetAuditableTypes()
	    {
			return Assembly.GetAssembly(typeof(IAuditable))
				.GetTypes()
				.Where(t => t.IsClass &&
				            typeof(IAuditable).IsAssignableFrom(t));
		}
    }
}
