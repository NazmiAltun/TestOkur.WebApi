namespace TestOkur.WebApi.Unit.Tests.Domain
{
    using FluentAssertions;
    using TestOkur.Domain.SeedWork;
    using Xunit;

    public class EnumerationTests
	{
		[Fact]
		public void Given_GetAll_ShouldReturnAllValues()
		{
			var all = Enumeration.GetAll<Foo>();
			all.Should().Contain(f => f.Id == 1 && f.Name == "Bar");
			all.Should().Contain(f => f.Id == 2 && f.Name == "Tar");
		}

		public class Foo : Enumeration
		{
			public static readonly Foo Bar = new Foo(1, "Bar");

			public static readonly Foo Tar = new Foo(2, "Tar");

			public Foo(int id, string name)
				: base(id, name)
			{
			}

			private Foo()
			{
			}
		}
	}
}
