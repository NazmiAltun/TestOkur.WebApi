namespace TestOkur.Test.Common
{
    using AutoFixture;
    using TestOkur.Optic.Form;

    public class TestOkurCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Customize<StudentOpticalForm>(x =>
                x.With(y => y.CityId, 1)
                    .With(y => y.ClassroomId, 1)
                    .With(y => y.DistrictId, 1));
        }
    }
}