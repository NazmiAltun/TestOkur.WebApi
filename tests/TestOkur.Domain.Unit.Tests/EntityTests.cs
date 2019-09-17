namespace TestOkur.WebApi.Unit.Tests.Domain
{
    using FluentAssertions;
    using TestOkur.Domain.SeedWork;
    using Xunit;

    public class EntityTests
    {
        [Fact]
        public void GivenEntity_WhenIdsAreSame_Then_TheyShouldBeEqual()
        {
            var foo1 = new Foo(5, "Bar1");
            var foo2 = new Foo(5, "Randomeqweq");

            (foo1 == foo2).Should().BeTrue();
        }

        [Fact]
        public void GivenEntity_WhenIdsAreDifferentAndPropertyValuesAreEqual_Then_TheyShouldNotBeEqual()
        {
            var foo1 = new Foo(1, "Bar");
            var foo2 = new Foo(2, "Bar");

            (foo1 == foo2).Should().BeFalse();
        }

        public class Foo : Entity
        {
            public Foo(int id, string name)
            {
                Id = id;
                Name = name;
            }

            public string Name { get; }
        }
    }
}
