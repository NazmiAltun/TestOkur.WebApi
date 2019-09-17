namespace TestOkur.WebApi.Unit.Tests.Domain
{
    using FluentAssertions;
    using TestOkur.Domain.SeedWork;
    using Xunit;

    public class ValueObjectTests
    {
        [Fact]
        public void GivenEqualityOperator_WhenObjectsHaveSameProperties_Then_ResultShouldBeTrue()
        {
            var foo1 = new Foo(8, "Bar");
            var foo2 = new Foo(8, "Bar");

            (foo1 == foo2).Should().BeTrue();
        }

        public class Foo : ValueObject
        {
            public Foo(int number, string name)
            {
                Number = number;
                Name = name;
            }

            public string Name { get; }

            public int Number { get; }
        }
    }
}
