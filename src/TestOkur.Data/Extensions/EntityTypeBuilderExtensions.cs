namespace Microsoft.EntityFrameworkCore.Metadata.Builders
{
    using System;
    using System.Linq.Expressions;
    using TestOkur.Domain.Model;

    internal static class EntityTypeBuilderExtensions
	{
		public static void OwnsEmail<TEntity>(
			this EntityTypeBuilder<TEntity> builder,
			Expression<Func<TEntity, Email>> navigationExpression)
			where TEntity : class
		{
			builder.OwnsOne(
				navigationExpression,
				email =>
				{
					email.Property(p => p.Value)
						.IsRequired()
						.HasMaxLength(255);
				});
		}

		public static void OwnsPhone<TEntity>(
			this EntityTypeBuilder<TEntity> builder,
			Expression<Func<TEntity, Phone>> navigationExpression)
			where TEntity : class
		{
			builder.OwnsOne(
				navigationExpression,
				phone =>
				{
					phone.Property(p => p.Value)
						.IsRequired()
						.HasMaxLength(20);
				});
		}

		public static void OwnsName<TEntity>(
			this EntityTypeBuilder<TEntity> builder,
			Expression<Func<TEntity, Name>> navigationExpression,
			int maxLength)
			where TEntity : class
		{
			builder.OwnsOne(
				navigationExpression,
				name =>
				{
					name.Property(p => p.Value)
						.IsRequired()
						.HasMaxLength(maxLength)
						.IsUnicode();
				});
		}
	}
}
