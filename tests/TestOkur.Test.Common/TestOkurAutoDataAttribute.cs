namespace TestOkur.Test.Common
{
    using AutoFixture;
    using AutoFixture.AutoMoq;
    using AutoFixture.Xunit2;

    public class TestOkurAutoDataAttribute : AutoDataAttribute
    {
        public TestOkurAutoDataAttribute()
            : base(() => new Fixture()
                .Customize(new TestOkurCustomization())
                .Customize(new AutoMoqCustomization()))
        {
        }
    }
}
